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

    [Command]
    void CmdUpdateTheme(megColorTheme t, int id)
    {
        //make these colour values the currently used values
        //colourThemeHolder.GetComponent<graphicsColourHolder>().theme = t;
        colourThemeHolder.GetComponent<graphicsColourHolder>().themeName = t.name;
        colourThemeHolder.GetComponent<graphicsColourHolder>().keyColor = t.keyColor;
        colourThemeHolder.GetComponent<graphicsColourHolder>().backgroundColor = t.backgroundColor;
        colourThemeHolder.GetComponent<graphicsColourHolder>().highlightColor = t.highlightColor;
        serverUtils.SetPlayerVessel(id);
    }

    void Update()
    {
        if (!isServer)
            return;

        if (colourThemeHolder == null)
        {
            colourThemeHolder = GameObject.FindWithTag("Inputs");
        }

        //get the button script this is attached to
        var buttonScript = gameObject.GetComponent<buttonControl>();
        if (buttonScript)
        {
            if (buttonScript.active)
            {
                CmdUpdateTheme(theme, vesselId);
            }
        }
    }
}
