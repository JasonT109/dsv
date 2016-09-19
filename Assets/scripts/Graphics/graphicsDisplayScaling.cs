using UnityEngine;
using System.Collections;

public class graphicsDisplayScaling : MonoBehaviour
{
    public graphicsDisplaySettings.ScreenMode Mode;
    public bool Scaled;

    private graphicsDisplaySettings.ScreenMode _oldMode;
    private bool _oldScaled;

    private static graphicsDisplaySettings Settings
        { get { return graphicsDisplaySettings.Instance; } }

    private void OnEnable()
    {
        if (!Settings)
            return;

        _oldMode = Settings.Mode;
        _oldScaled = Settings.Scaled;

        Settings.Configure(Mode, Scaled);
    }

    private void OnDisable()
    {
        if (Settings)
            Settings.Configure(_oldMode, _oldScaled);
    }

}
