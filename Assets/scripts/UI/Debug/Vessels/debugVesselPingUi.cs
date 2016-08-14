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

    [Header("Colors")]

    public Color NormalColor;
    public Color SelectedColor;

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
            VesselData.SetPosition(Vessel.Id, Ping.ToVesselSpace());
    }

    // Public Methods
    // ------------------------------------------------------------

    /** Select this ping's vessel. */
    public void Select()
        { debugVesselsUi.Instance.Selected = Vessel; }


}
