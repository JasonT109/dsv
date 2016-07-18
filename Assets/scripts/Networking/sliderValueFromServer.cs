using UnityEngine;
using System.Collections;
using Meg.Networking;

public class sliderValueFromServer : MonoBehaviour
{

    public string linkDataString = "depth";
    public float updateTick = 0.2f;

    private float nextUpdate = 0;
    private sliderWidget _slider;

    void Start()
    {
        _slider = GetComponentInChildren<sliderWidget>();
        nextUpdate = Time.time;
    }

    void Update()
    {
        if (!_slider)
            return;

        if (Time.time < nextUpdate)
            return;

        nextUpdate = Time.time + updateTick;
        _slider.SetValue(serverUtils.GetServerData(linkDataString));
    }

}
