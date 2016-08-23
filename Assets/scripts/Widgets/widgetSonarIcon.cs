using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetSonarIcon : MonoBehaviour
{
    public Texture2D subIcon;
    public Texture2D gliderIcon;

    public Renderer r;

    void Start ()
    {
        if (!r)
            r = GetComponent<Renderer>();
    }

    void Update ()
    {
        if (serverUtils.GetPlayerVessel() == 1 || serverUtils.GetPlayerVessel() == 2)
        {
            r.material.SetTexture("_MainTex", subIcon);
        }
        else
        {
            r.material.SetTexture("_MainTex", gliderIcon);
        }
    }
}

