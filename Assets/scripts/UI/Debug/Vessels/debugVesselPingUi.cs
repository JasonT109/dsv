using System;
using UnityEngine;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;
using TouchScript.Gestures;
using UnityEngine.UI;

/** 
 * Represents a vessel in the debug map interface viewport.
 * 
 * Vessel pings can be directly selected, and dragged around to reposition
 * the corresponding vessel in the simulation space.
 * 
 * Pings will also update in realtime during playback, and have a simple
 * arrow visualization to help understand the vessel's current movement state.
 * 
 */

public class debugVesselPingUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public SonarPing Ping;
    public TransformGesture TransformGesture;
    public PressGesture PressGesture;
    public Text NameLabel;
    public Graphic Icon;
    public Image Arrow;


    [Header("Colors")]

    public Color NormalColor;
    public Color SelectedColor;

    [Header("Movements")]

    public float MinScale = 0.1f;
    public float MaxScale = 1;


    public vesselData.Vessel Vessel
        { get { return Ping.Vessel; } }


    // Private Properties
    // ------------------------------------------------------------

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }


    // Members
    // ------------------------------------------------------------

    /** Graphical elements in the ping. */
    private Graphic[] _graphics;


    // Unity Methods
    // ------------------------------------------------------------

    private void Awake()
    {
        if (!Ping)
            Ping = GetComponent<SonarPing>();
        if (!TransformGesture)
            TransformGesture = GetComponent<TransformGesture>();
        if (!PressGesture)
            PressGesture = GetComponent<PressGesture>();

        if (Arrow)
            Arrow.gameObject.SetActive(false);

        if (PressGesture)
            PressGesture.Pressed += OnPressed;

        _graphics = transform.GetComponentsInChildren<Graphic>();
    }

    void Update()
    {
        TransformGesture.enabled = VesselData.isServer
            && !megEventManager.Instance.Playing;

        // Determine the proper color for this ping.
        var selected = debugVesselsUi.Instance.Selected.Id == Vessel.Id;
        var color = Ping.Color;
        if (selected)
            color = HSBColor.FromColor(color).Brighten(1.5f).ToColor();
        if (!Ping.Visible)
            color.a *= 0.5f;

        // Update UI elements accordingly.
        for (var i = 0; i < _graphics.Length; i++)
            _graphics[i].color = color;

        // NameLabel.color = color;
        // Icon.color = color;

        var transforming = TransformGesture.State == Gesture.GestureState.Changed;
	    Ping.enabled = !transforming;
        Ping.AutoPulse(selected ? 1 : 0);

        // Push selected ping in front of others so it gets preferentially transformed.
        Ping.DepthOffset = selected ? -1 : 0;

        if (transforming)
        {
            if (debugVesselsUi.Instance.Selected.Id != Vessel.Id)
                debugVesselsUi.Instance.Selected = Vessel;

            VesselData.SetPosition(Vessel.Id, Ping.ToVesselSpace());
        }
    }

    void LateUpdate()
    {
        if (Arrow)
            UpdateArrow();
    }

    // Public Methods
    // ------------------------------------------------------------

    /** Select this ping's vessel. */
    public void Select()
        { debugVesselsUi.Instance.Selected = Vessel; }


    // Private Methods
    // ------------------------------------------------------------

    private void OnPressed(object sender, EventArgs e)
        { Select(); }

    private void UpdateArrow()
    {
        var movement = Vessel.Movement;
        Arrow.gameObject.SetActive(movement);
        if (!movement)
            return;

        var v = movement.Velocity;
        var speed = v.magnitude;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        var elevation = 0f;
        if (!Mathf.Approximately(speed, 0))
            elevation = Mathf.Asin(v.z / speed) * Mathf.Rad2Deg;

        Arrow.rectTransform.localRotation = Quaternion.Euler(0, 0, angle) * Quaternion.Euler(0, elevation, 0);

        // Determine the arrow's color.
        var selected = debugVesselsUi.Instance.Selected.Id == Vessel.Id;
        var color = selected ? SelectedColor : NormalColor;
        if (Ping.Pings.Space == SonarPings.DisplaySpace.Sonar && !Vessel.OnSonar)
            color.a *= 0.5f;
        else if (Ping.Pings.Space == SonarPings.DisplaySpace.Vessel && !Vessel.OnMap)
            color.a *= 0.5f;

        var w = movement.WorldVelocity;
        var max = serverUtils.GetServerData("maxspeed");
        var t = Mathf.Clamp01(w.magnitude / max);
        var scale = Mathf.Lerp(MinScale, MaxScale, t);

        Arrow.color = color;
        Arrow.transform.localScale = Vector3.one * scale;
    }


}
