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

    /** Prefab to use when instantiating popup widgets. */
    public widgetPopup PopupPrefab;


    // Enumerations
    // ------------------------------------------------------------

    /** Possible popup icons. */
    public enum Icon
    {
        None,
        Question,
        Exclamation,
        Forbidden
    }

    // Structures
    // ------------------------------------------------------------

    /** Structure defining an on-screen popup. */
    [Serializable]
    public struct Popup
    {
        public string Title;
        public string Target;
        public Vector3 Position;
        public Icon Icon;

        /** Constructor. */
        public Popup(Popup popup)
        {
            Title = popup.Title;
            Target = popup.Target;
            Position = popup.Position;
            Icon = popup.Icon;
        }

        /** Equality operator. */
        public override bool Equals(object o)
            { return (o is Popup) && Equals((Popup) o); }

        /** Equality operator. */
        public bool Equals(Popup other)
        {
            return string.Equals(Title, other.Title) 
                && string.Equals(Target, other.Target) 
                && Position.Equals(other.Position) 
                && Icon == other.Icon;
        }

        /** Hashcode (used for collection keys). */
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Icon;
                return hashCode;
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
        { return _popupTargets.TryGetValue(id, out target); }


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        var targets = ObjectFinder.FindAll<PopupTarget>();
        foreach (var target in targets)
            _popupTargets[target.Id] = target;
    }

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
            _popupWidgets[popup] = Instantiate(PopupPrefab);
            _popupWidgets[popup].Show(popup);
        }
    }

}
