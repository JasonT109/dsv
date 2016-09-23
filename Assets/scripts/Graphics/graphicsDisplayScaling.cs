using UnityEngine;
using System.Collections;

public class graphicsDisplayScaling : MonoBehaviour
{
    public graphicsDisplaySettings.ScreenMode Mode;
    public bool Scaled;

    private graphicsDisplaySettings.ScreenMode _oldMode;
    private bool _oldScaled;

    private bool _quitting;

    private static graphicsDisplaySettings Settings
        { get { return graphicsDisplaySettings.Instance; } }

    private void OnEnable()
    {
        if (!Settings || Settings.Overridden)
            return;

        _oldMode = Settings.Mode;
        _oldScaled = Settings.Scaled;

        Settings.Configure(Mode, Scaled);
    }

    void OnApplicationQuit()
        { _quitting = true; }

    private void OnDisable()
    {
        if (_quitting || !Settings || Settings.Overridden)
            return;

        Settings.Configure(_oldMode, _oldScaled);
    }

}
