using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Networking;
using UnityEngine.Networking;

public class popupData : NetworkBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** INterval between successive popup updates. */
    public const float UpdateInterval = 0.25f;


    // Properties
    // ------------------------------------------------------------


    [Header("Prefabs")]

    /** The set of available popup types. */
    public PopupType[] Types;

    /** Prefab to use for glider bootup popup (left screen). */
    public widgetPopup PopupBootGliderLeft;

    /** Prefab to use for glider bootup popup (middle screen). */
    public widgetPopup PopupBootGliderMid;

    /** Prefab to use for glider bootup popup (right screen). */
    public widgetPopup PopupBootGliderRight;


    // Enumerations
    // ------------------------------------------------------------

    /** Possible popup types. */
    public enum Type
    {
        Info,
        GreenScreen,
        Boot,
        BootLowPower,
        Warning,
        Themed
    }

    /** Possible popup icons. */
    public enum Icon
    {
        None,
        Question,
        Exclamation,
        Forbidden,
        Dots
    }

    // Structures
    // ------------------------------------------------------------

    /** Structure defining a type of popup. */
    [Serializable]
    public struct PopupType
    {
        public string Name;
        public Type Type;
        public string Theme;
        public widgetPopup Prefab;
    }


    /** Structure defining an on-screen popup. */
    [Serializable]
    public struct Popup
    {
        public Type Type;
        public string Theme;
        public string Title;
        public string Message;
        public string Target;
        public Vector3 Position;
        public Vector2 Size;
        public Vector3 Scale;
        public Icon Icon;
        public Color Color;

        /** Constructor. */
        public Popup(Popup popup)
        {
            Type = popup.Type;
            Theme = popup.Theme;
            Title = popup.Title;
            Message = popup.Message;
            Target = popup.Target;
            Position = popup.Position;
            Size = popup.Size;
            Scale = popup.Scale;
            Icon = popup.Icon;
            Color = popup.Color;
        }

        /** Equality operator. */
        public override bool Equals(object o)
            { return (o is Popup) && Equals((Popup) o); }

        /** Equality operator. */
        public bool Equals(Popup other)
        {
            return Type.Equals(other.Type)
                && string.Equals(Theme, other.Theme)
                && string.Equals(Title, other.Title)
                && string.Equals(Message, other.Message)
                && string.Equals(Target, other.Target) 
                && Position.Equals(other.Position) 
                && Size.Equals(other.Size)
                && Scale.Equals(other.Scale)
                && Icon == other.Icon
                && Color == other.Color;
        }

        /** Hashcode (used for collection keys). */
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode * 397) ^ (Theme != null ? Theme.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ Scale.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Icon;
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        /** Returns whether a given icon can be applied to this icon. */
        public bool IsIconValid(Icon icon)
        {
            switch (Type)
            {
                case Type.Info:
                    return icon != Icon.Dots;
                case Type.GreenScreen:
                    return icon == Icon.None || icon == Icon.Dots;
                case Type.Boot:
                case Type.BootLowPower:
                case Type.Warning:
                case Type.Themed:
                    return false;
                default:
                    return true;
            }
        }

        /** Whether popup's title can be changed. */
        public bool CanSetTitle
        {
            get
            {
                switch (Type)
                {
                    case Type.Info:
                    case Type.Boot:
                    case Type.BootLowPower:
                    case Type.Warning:
                    case Type.Themed:
                        return true;
                    case Type.GreenScreen:
                        return false;
                    default:
                        return true;
                }
            }
        }

        /** Whether popup's message can be changed. */
        public bool CanSetMessage
            { get { return CanSetTitle; } }

        /** Whether popup's color can be changed. */
        public bool CanSetColor
        {
            get
            {
                switch (Type)
                {
                    case Type.Info:
                    case Type.Warning:
                    case Type.Themed:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /** Whether popup's position can be changed. */
        public bool CanSetPosition
        {
            get
            {
                switch (Type)
                {
                    case Type.Info:
                    case Type.GreenScreen:
                    case Type.Warning:
                    case Type.Themed:
                        return true;
                    case Type.Boot:
                    case Type.BootLowPower:
                        return false;
                    default:
                        return true;
                }
            }
        }

        /** Whether popup has a title. */
        public bool HasTitle
            { get { return !string.IsNullOrEmpty(Title); } }

        /** Whether popup has a message. */
        public bool HasMessage
            { get { return !string.IsNullOrEmpty(Message); } }

    };

    /** Class definition for a synchronized list of popups. */
    public class SyncListPopups : SyncListStruct<Popup> { };


    // Synchronization
    // ------------------------------------------------------------

    #region Synchronization

    [Header("Synchronization")]

    /** Synchronized list for holding popup state. */
    public SyncListPopups Popups = new SyncListPopups();

    /** Duration of the bootup sequence's 'systems online' section. */
    [SyncVar]
    public float bootCodeDuration = 3.0f;

    /** Bootup sequence's progress in the 'systems online' section. */
    [SyncVar]
    public float bootProgress = 0f;

    #endregion


    // Members
    // ------------------------------------------------------------

    /** Lookup table for popup targets. */
    private readonly Dictionary<string, PopupTarget> _popupTargets = new Dictionary<string, PopupTarget>();

    /** Local set of popup representations. */
    private readonly Dictionary<Popup, widgetPopup> _popupWidgets = new Dictionary<Popup, widgetPopup>();

    /** Timestamp for next popup collection update. */
    private float _nextUpdateTime;


    // Public Methods
    // ------------------------------------------------------------

    /** Add a popup. */
    [Server]
    public void AddPopup(Popup popup)
    {
        if (Popups.Contains(popup))
            return;

        Popups.Add(popup);
    }

    /** Toggle a popup. */
    [Server]
    public void TogglePopup(Popup popup)
    {
        if (Popups.Contains(popup))
            RemovePopup(popup);
        else
            AddPopup(popup);
    }

    /** Remove a popup. */
    [Server]
    public void RemovePopup(Popup popup)
    {
        Popups.Remove(popup);
    }

    /** Clear all popups. */
    [Server]
    public void Clear()
    {
        Popups.Clear();
    }

    /** Determines if a given popup is active. */
    public bool IsPopupActive(Popup popup)
    {
        return Popups.Contains(popup);
    }

    /** Look up a popup target by id. */
    public bool TryGetTarget(string id, out PopupTarget target)
    {
        if (!string.IsNullOrEmpty(id))
            return _popupTargets.TryGetValue(id.ToLower(), out target);

        target = null;
        return false;
    }
    
    /** Register a popup target. */
    public void RegisterTarget(PopupTarget target)
        { _popupTargets[target.PopupId.ToLower()] = target; }

    /** Unregister a popup target. */
    public void UnregisterTarget(PopupTarget target)
        { _popupTargets.Remove(target.PopupId.ToLower()); }

    /** Return a type entry. */
    public PopupType FindPopupType(Type type, string theme)
    {
        if (string.IsNullOrEmpty(theme))
            return Types.FirstOrDefault(t => t.Type == type);

        return Types.FirstOrDefault(t => t.Type == type && t.Theme == theme);
    }

    /** Return a type entry. */
    public PopupType PopupTypeForName(string name)
        { return Types.FirstOrDefault(t => t.Name == name); }


    // Unity Methods
    // ------------------------------------------------------------

    /** Per-frame update. */
    private void Update()
    {
        if (Time.time < _nextUpdateTime)
            return;

        _nextUpdateTime = Time.time + UpdateInterval;
        UpdatePopups();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the popup widget collection to match backing shared state. */
    private void UpdatePopups()
    {
        // Hide any outdated popups
        var oldPopups = _popupWidgets.Keys.Where(key => !Popups.Contains(key)).ToList();
        foreach (var popup in oldPopups)
        {
            _popupWidgets[popup].Hide();
            _popupWidgets.Remove(popup);
        }

        // Create new popups.
        var newPopups = Popups.Where(key => !_popupWidgets.ContainsKey(key)).ToList();
        foreach (var popup in newPopups)
        {
            var prefab = GetPrefabForPopup(popup);
            if (!prefab)
                continue;

            var go = Instantiate(prefab);
            if (!go)
                continue;

            go.Show(popup);
            _popupWidgets[popup] = go;
        }
    }

    /** Return a prefab to use for the given popup type. */
    private widgetPopup GetPrefabForPopup(Popup popup)
    {
        var type = popup.Type;
        if (type == Type.Boot && serverUtils.IsGlider())
            return GetPrefabForGliderBoot(popup);

        if (string.IsNullOrEmpty(popup.Theme))
            return Types.FirstOrDefault(t => t.Type == type).Prefab;

        return Types.FirstOrDefault(t => t.Type == type 
            && t.Theme == popup.Theme).Prefab;
    }

    private widgetPopup GetPrefabForGliderBoot(Popup popup)
    { 
        // Check that glider screen manager exists.
        if (!glScreenManager.Instance)
            return PopupBootGliderMid;

        // Boot sequence is different for each glider screen.
        var screen = glScreenManager.Instance.screenID; 
        switch (screen)
        {
            case 0:
                return PopupBootGliderRight;
            case 1:
                return PopupBootGliderMid;
            case 2:
                return PopupBootGliderLeft;
            default:
                return PopupBootGliderMid;
        }
    }

}
