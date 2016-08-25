using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Meg.Graphics;

public class graphicsColourHolder : NetworkBehaviour
{
    public megColorTheme theme;
    [SyncVar]
    public string themeName;
    [SyncVar]
    public Color backgroundColor;
    [SyncVar]
    public Color keyColor;
    [SyncVar]
    public Color highlightColor;

    void Start()
    {
        theme = new megColorTheme();
        theme.name = "Default";
    }

    void Update()
    {
        theme.name = themeName;
        theme.backgroundColor = backgroundColor;
        theme.keyColor = keyColor;
        theme.highlightColor = highlightColor;
    }
}