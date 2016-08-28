using UnityEngine;
using System.Collections;
using Meg.Networking;
using Meg.Maths;

public class graphicsColorFromServerValue : MonoBehaviour
{
    public string serverValue = "depth";
    public float minServerValue = 0;
    public float maxServerValue = 100;
    public Color minColor;
    public Color maxColor;
    public bool usingTintColor = false;
    public widgetThrusterControl thrusterControl;
    public bool useThrusterPower = false;
    public widgetThrusterControl.ThrusterId thruster;
    private Renderer r;
    private Material m;

    void SetColor()
    {
        float blendValue = 0;

        if (useThrusterPower)
        {
            blendValue = graphicsMaths.remapValue(thrusterControl.GetThrusterPowerOutput(thruster), minServerValue, maxServerValue, 0, 1);
        }
        else
        {
            blendValue = graphicsMaths.remapValue(serverUtils.GetServerData(serverValue), minServerValue, maxServerValue, 0, 1);
        }

        Color blendColor = Color.Lerp(minColor, maxColor, blendValue);

        if (usingTintColor)
            m.SetColor("_TintColor", blendColor);
        else
            m.SetColor("_MainColor", blendColor);
    }

    private void LocateThrusterControl()
    {
        // Locate the thruster control script (should be in a parent node).
        if (!thrusterControl)
            thrusterControl = ObjectFinder.FindInParents<widgetThrusterControl>(transform);
        if (!thrusterControl)
            thrusterControl = ObjectFinder.Find<widgetThrusterControl>();
    }

    void OnEnable()
    {
        LocateThrusterControl();

        r = gameObject.GetComponent<Renderer>();
        m = r.material;
        SetColor();
    }

    // Use this for initialization
    void Start ()
    {
        LocateThrusterControl();

        r = gameObject.GetComponent<Renderer>();
        m = r.material;
        SetColor();
    }
	
	// Update is called once per frame
	void Update ()
    {
        SetColor();
    }
}
