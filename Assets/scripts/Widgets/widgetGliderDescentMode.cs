using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetGliderDescentMode : MonoBehaviour
{
    public buttonControl descent;
    public buttonControl cruise;
    public buttonControl ascend;

    private float _nextButtonUpdate;

    private const float PressTimeout = 0.25f;
    
	void Start ()
    {
        descent.onPress.AddListener(OnDescentClicked);
        cruise.onPress.AddListener(OnCruiseClicked);
        ascend.onPress.AddListener(OnAscendClicked);
    }

    private void OnDescentClicked()
    {
        serverUtils.PostServerData("descentmodevalue", 90);
        _nextButtonUpdate = Time.time + PressTimeout;
    }

    private void OnCruiseClicked()
    {
        serverUtils.PostServerData("descentmodevalue", 0);
        _nextButtonUpdate = Time.time + PressTimeout;
    }

    private void OnAscendClicked()
    {
        serverUtils.PostServerData("descentmodevalue", -90);
        _nextButtonUpdate = Time.time + PressTimeout;
    }


    void Update ()
	{
	    var target = serverUtils.GetServerData("descentmodevalue", 0);
        var ascending = target < -5;
        var descending = target > 5;

        if (Time.time >= _nextButtonUpdate)
        {
            ascend.active = ascending;
            descent.active = descending;
            cruise.active = !(ascending || descending);
        }
        
	}
}
