using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class debugMapUi : MonoBehaviour
{
    public enum EditingMode
    {
        Vessels,
        Lines
    }

    public debugVesselsUi Vessels;
    public debugVesselPropertiesUi VesselProperties;

    public debugMapLinesUi Lines;
    public debugMapLinePropertiesUi LineProperties;

    public Toggle VesselsToggle;
    public Toggle LinesToggle;

    public EditingMode Mode = EditingMode.Vessels;

    private bool IsVessels
        { get { return Mode == EditingMode.Vessels; } }

    private bool IsLines
        { get { return Mode == EditingMode.Lines; } }

    private bool _updating;

    void Start()
    {
        VesselsToggle.onValueChanged.AddListener(SetVessels);
        LinesToggle.onValueChanged.AddListener(SetLines);
    }

    void Update()
    {
        _updating = true;

        VesselsToggle.isOn = IsVessels;
        Vessels.gameObject.SetActive(IsVessels);
        VesselProperties.gameObject.SetActive(IsVessels);

        LinesToggle.isOn = IsLines;
        Lines.gameObject.SetActive(IsLines);
        LineProperties.gameObject.SetActive(IsLines);

        _updating = false;
    }

    public void SetVessels(bool value)
    {
        if (_updating)
            return;

        Mode = EditingMode.Vessels;
        Update();
    }

    public void SetLines(bool value)
    {
        if (_updating)
            return;

        Mode = EditingMode.Lines;
        Update();
    }

}
