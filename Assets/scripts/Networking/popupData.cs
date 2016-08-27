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

    /** Prefab to use when instantiating warning popups. */
    public widgetPopup PopupInfoPrefab;

    /** Prefab to use when instantiating greenscreen popups. */
    public widgetPopup PopupGreenPrefab;

    /** Prefab to use for bootup popup. */
    public widgetPopup PopupBoot;

    /** Prefab to use for low power bootup popup. */
    public widgetPopup PopupBootLowPower;


    // Enumerations
    // ------------------------------------------------------------

    /** Possible popup types. */
    public enum Type
    {
        Info,
        GreenScreen,
        Boot,
        BootLowPower
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

    /** Structure defining an on-screen popup. */
    [Serializable]
    public struct Popup
    {
        public Type Type;
        public string Title;
        public string Message;
        public string Target;
        public Vector3 Position;
        public Vector2 Size;
        public Icon Icon;
        public Color Color;

        /** Constructor. */
        public Popup(Popup popup)
        {
            Type = popup.Type;
            Title = popup.Title;
            Message = popup.Message;
            Target = popup.Target;
            Position = popup.Position;
            Size = popup.Size;
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
                && string.Equals(Title, other.Title)
                && string.Equals(Message, other.Message)
                && string.Equals(Target, other.Target) 
                && Position.Equals(other.Position) 
                && Size.Equals(other.Size)
                && Icon == other.Icon
                && Color == other.Color;
        }

        /** Hashcode (used for collection keys). */
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
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
            { get { return Type == Type.Info; } }

        /** Whether popup's color can be changed. */
        public bool CanSetColor
            { get { return Type == Type.Info; } }

        /** Whether popup's position can be changed. */
        public bool CanSetPosition
        {
            get
            {
                switch (Type)
                {
                    case Type.Info:
                    case Type.GreenScreen:
                        return true;
                    case Type.Boot:
                    case Type.BootLowPower:
                        return false;
                    default:
                        return true;
                }
            }
        }

    };

    /** Class definition for a synchronized list of popups. */
    public class SyncListPopups : SyncListStruct<Popup> { };


    // Synchronization
    // ------------------------------------------------------------

    #region Synchronization

    [Header("Synchronization")]

    /** Synchronized list for holding popup state. */
    public SyncListPopups Popups = new SyncListPopups();

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
        { _popupTargets[target.Id.ToLower()] = target; }

    /** Unregister a popup target. */
    public void UnregisterTarget(PopupTarget target)
        { _popupTargets.Remove(target.Id.ToLower()); }


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
            var go = Instantiate(GetPrefabForPopup(popup));
            if (!go)
                continue;

            go.Show(popup);
            _popupWidgets[popup] = go;
        }
    }

    /** Return a prefab to use for the given popup type. */
    private widgetPopup GetPrefabForPopup(Popup popup)
    {
        switch (popup.Type)
        {
            case Type.Info:
                return PopupInfoPrefab;
            case Type.GreenScreen:
                return PopupGreenPrefab;
            case Type.Boot:
                return PopupBoot;
            case Type.BootLowPower:
                return PopupBootLowPower;
            default:
                return PopupInfoPrefab;
        }
    }


}
