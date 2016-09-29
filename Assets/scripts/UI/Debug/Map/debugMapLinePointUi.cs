using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.UI;

public class debugMapLinePointUi : MonoBehaviour
{

    public int Index;
    public Vector3 Point;

    public debugMapLinePropertiesUi LineUi { get; set; }

    public InputField XInput;
    public InputField YInput;
    public InputField ZInput;

    private bool _updating;

    private void Start()
    {
        XInput.onEndEdit.AddListener(OnXInputEndEdit);
        YInput.onEndEdit.AddListener(OnYInputEndEdit);
        ZInput.onEndEdit.AddListener(OnZInputEndEdit);

        XInput.text = Point.x.ToString();
        YInput.text = Point.y.ToString();
        ZInput.text = Point.z.ToString();
    }

    public void Update()
    {
        _updating = true;

        var previous = Point;
        Point = LineUi.GetPoint(Index);

        if (!Mathf.Approximately(Point.x, previous.x))
            XInput.text = Point.x.ToString();
        if (!Mathf.Approximately(Point.y, previous.y))
            YInput.text = Point.y.ToString();
        if (!Mathf.Approximately(Point.z, previous.z))
            ZInput.text = Point.z.ToString();

        _updating = false;
    }


    private void OnXInputEndEdit(string value)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        Point.x = result;
        LineUi.SetPoint(Index, Point);
    }

    private void OnYInputEndEdit(string value)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        Point.y = result;
        LineUi.SetPoint(Index, Point);
    }

    private void OnZInputEndEdit(string value)
    {
        if (_updating)
            return;

        float result;
        if (!float.TryParse(value, out result))
            return;

        Point.z = result;
        LineUi.SetPoint(Index, Point);
    }


}
