using System;
using UnityEngine;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;
using TouchScript.Behaviors;
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

    // Constants
    // ------------------------------------------------------------

    public const float MaxSpeedDistance = 200f;
    public const float ArrowSpeedScale = 1 / 100.0f;
    public const float ArrowMinScale = 0.05f;
    public const float ArrowMaxScale = 1.5f;


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public SonarPing Ping;

    public Transformer Transformer;
    public TransformGesture TransformGesture;
    public PressGesture PressGesture;
    public ReleaseGesture ReleaseGesture;
    public Text NameLabel;
    public Graphic Icon;
    public Image Arrow;


    [Header("Colors")]

    public Color NormalColor;
    public Color SelectedColor;


    // Private Properties
    // ------------------------------------------------------------

    private vesselMovements Movements
        { get { return serverUtils.VesselMovements; } }

    private vesselMovement Movement
        { get { return Movements ? Movements.GetVesselMovement(Vessel.Id) : null; } }

    private vesselSetVector SetVector
        { get { return Movement as vesselSetVector; } }

    private vesselData.Vessel Vessel
        { get { return Ping.Vessel; } }


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
            TransformGesture = Transformer.GetComponent<TransformGesture>();
        if (!PressGesture)
            PressGesture = Transformer.GetComponent<PressGesture>();
        if (!ReleaseGesture)
            ReleaseGesture = Transformer.GetComponent<ReleaseGesture>();

        if (Arrow)
            Arrow.gameObject.SetActive(false);

        if (PressGesture)
            PressGesture.Pressed += OnPressed;
        if (ReleaseGesture)
            ReleaseGesture.Released += OnReleased;

        _graphics = transform.GetComponentsInChildren<Graphic>();
    }

    void Update()
    {
        // Only allow pings to be manipulated when debug screen is open.
        var active = debugVesselsUi.Instance.gameObject.activeInHierarchy;
        Transformer.gameObject.SetActive(active);
        TransformGesture.enabled = active;
        PressGesture.enabled = active;
        ReleaseGesture.enabled = active;
        if (!active)
            return;

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

        var transforming = TransformGesture.State == Gesture.GestureState.Changed;
	    Ping.enabled = !transforming;
        Ping.AutoPulse(selected ? 1 : 0);

        // Push selected ping in front of others so it gets preferentially transformed.
        Ping.DepthOffset = selected ? -1 : 0;

        // Reparent transformer to be a sibling of the ping.
        // This allows us to decouple transform interactions from the ping visuals.
        if (Transformer.transform.parent != Ping.transform.parent)
            Transformer.transform.SetParent(Ping.transform.parent);

        // Update vessel during transform interactions.
        if (transforming)
            UpdateWhileTransforming();
        else
            Transformer.transform.localPosition = Ping.transform.localPosition;
    }

    void OnDestroy()
    {
        // Clean up transformer (since we decoupled it and made it a sibling).
        if (Transformer)
            Destroy(Transformer.gameObject);
    }

    private void UpdateWhileTransforming()
    {
        // UPdate the current selection.
        if (debugVesselsUi.Instance.Selected.Id != Vessel.Id)
            debugVesselsUi.Instance.Selected = Vessel;

        // If vessel simulation is inactive, directly manipulate vessel position.
        var moving = serverUtils.VesselMovements.Active;
        if (!moving)
        {
            Ping.transform.localPosition = Transformer.transform.localPosition;
            serverUtils.PostVesselPosition(Vessel.Id, Ping.ToVesselSpace());
        }

        // If already moving, affect vessel movement vector.
        if (moving)
            UpdateVectorFromPing();
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
    {
        Select();
    }

    private void OnReleased(object sender, EventArgs e)
    {
    }

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
        var scale = Mathf.Clamp(w.magnitude * ArrowSpeedScale, ArrowMinScale, ArrowMaxScale);

        Arrow.color = color;
        Arrow.transform.localScale = Vector3.one * scale;
    }

    private void UpdateVectorFromPing()
    {
        // Don't allow player vessel to be moved if connected to motion base.
        var isPlayerVessel = Vessel.Id == serverUtils.GetPlayerVessel();
        if (isPlayerVessel && serverUtils.GetServerBool("motionComPortOpen"))
            return;

        // If dragging vessel during playback, make sure it's in vector movement mode.
        if (!SetVector)
            serverUtils.PostVesselMovementType(Vessel.Id, vesselMovements.SetVectorType);

        var vector = SetVector;
        if (!vector)
            return;

        var p = Transformer.transform.localPosition - Ping.transform.localPosition;
        vector.Heading = Mathf.Repeat(90 - Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg, 360);
        vector.Speed = Mathf.Clamp01(p.magnitude / MaxSpeedDistance) * vector.MaxSpeed;

        // Post the modified state to server if needed.
        if (!vector.isServer)
            vector.PostState();
    }
}
