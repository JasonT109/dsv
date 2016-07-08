using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Graphics;
using Meg.Networking;

public class graphicsColourPalette : NetworkBehaviour
{
    public megColorTheme theme;
    public GameObject colourThemeHolder;
    public int vesselId = 1;

    void setColorTheme()
    {
        //get the button script this is attached to
        var buttonScript = gameObject.GetComponent<buttonControl>();
        if (buttonScript)
        {
            if (buttonScript.active)
            {
                serverUtils.SetPlayerVessel(vesselId);
                serverUtils.SetColorTheme(theme);
            }
        }
    }

    void Update()
    {
        setColorTheme();
    }
}
