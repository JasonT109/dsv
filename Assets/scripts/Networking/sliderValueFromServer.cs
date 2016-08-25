using UnityEngine;
using System.Collections;
using Meg.Networking;

public class sliderValueFromServer : valueFromServer
{

    void Start()
    {
        Target = GetComponentInChildren<sliderWidget>();
    }

}
