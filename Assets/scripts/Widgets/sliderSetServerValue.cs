using UnityEngine;
using System.Collections;
using Meg.Networking;

public class sliderSetServerValue : MonoBehaviour
{
    public sliderWidget slider;
    public string serverValue = "depth";
    public float updateTick = 0.2f;
    private float updateTimer = 0;

	// Use this for initialization
	void Start ()
    {
        if (!slider)
            slider = GetComponent<sliderWidget>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time < updateTimer)
            return;

        updateTimer += updateTick;

        serverUtils.PostServerData(serverValue, slider.returnValue);

	}
}
