using UnityEngine;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;
using TouchScript.Gestures;
using UnityEngine.UI;

public class debugVesselPingUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    public SonarPing Ping;
    public TransformGesture TransformGesture;
    public Text NameLabel;
    public Graphic Icon;
    public Image Arrow;


    [Header("Colors")]

    public Color NormalColor;
    public Color SelectedColor;

    [Header("Movements")]

    public Vector3 VelocityScale = Vector3.one;


    public vesselData.Vessel Vessel
        { get { return Ping.Vessel; } }


    // Private Properties
    // ------------------------------------------------------------

    /** Vessel data. */
    private vesselData VesselData
        { get { return serverUtils.VesselData; } }


    // Unity Methods
    // ------------------------------------------------------------

    private void Awake()
    {
        if (!Ping)
            Ping = GetComponent<SonarPing>();
        if (!TransformGesture)
            TransformGesture = GetComponent<TransformGesture>();
    }

    void Update()
    {
        TransformGesture.enabled = VesselData.isServer
            && !megEventManager.Instance.Playing;

        var selected = debugVesselsUi.Instance.Selected.Id == Vessel.Id;
        var color = selected ? SelectedColor : NormalColor;
        if (!Vessel.OnSonar)
            color.a *= 0.5f;

        NameLabel.color = color;
        Icon.color = color;

        var transforming = TransformGesture.State == Gesture.GestureState.Changed;
	    Ping.enabled = !transforming;

        if (transforming)
        {
            if (debugVesselsUi.Instance.Selected.Id != Vessel.Id)
                debugVesselsUi.Instance.Selected = Vessel;

            VesselData.SetPosition(Vessel.Id, Ping.ToVesselSpace());
        }

        if (Arrow)
        {
            Arrow.color = color;
            UpdateArrow();
        }
    }

    // Public Methods
    // ------------------------------------------------------------

    /** Select this ping's vessel. */
    public void Select()
        { debugVesselsUi.Instance.Selected = Vessel; }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateArrow()
    {
        var movement = Vessel.Movement;
        Arrow.gameObject.SetActive(movement);
        if (!movement)
            return;

        var v = Vector3.Scale(movement.Velocity, VelocityScale);
        var p = Vessel.Position + v;
        var s = Ping.Pings.VesselToPingSpace(p);
        var delta = s - p;

        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        // var elevation = 0; // Mathf.Sin(delta.z) * Mathf.Rad2Deg;
        Arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle); // * Quaternion.Euler(elevation, 0, 0);
    }


}
