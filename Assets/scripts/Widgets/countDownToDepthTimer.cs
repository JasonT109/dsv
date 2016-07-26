using UnityEngine;
using System.Collections;
using Meg.Networking;

public class countDownToDepthTimer : widgetText
{
    public enum Type
    {
        Surface,
        Floor
    }

    public Type Target;
    public float Tolerance = 1;
    public float EtaMaxHours = 1;
    public float UpdateInterval = 0.2f;

    private float _nextUpdateTime;

    private void Update()
    {
        if (Time.time < _nextUpdateTime)
            return;

        _nextUpdateTime = Time.time + UpdateInterval;

        var velocity = serverUtils.GetServerData("verticalVelocity");
        var distance = serverUtils.GetServerData(Target == Type.Floor ? "floorDistance" : "depth");
        var arrived = distance <= Tolerance;
	    var moving = Mathf.Abs(velocity) > 0.01f;
        var eta = (moving && !arrived) ? distance / Mathf.Abs(velocity) : 0;
        var progressing = Target == Type.Floor ? velocity < -0.01f : velocity > 0.01f;
        
        // Check for invalid ETA (e.g. heading in wrong direction, taking too long.)
	    if (!progressing || eta > (EtaMaxHours * 3600))
	    {
	        Text = "N/A";
            return;
	    }

        var span = System.TimeSpan.FromSeconds(eta);
	    Text = string.Format("{0:00}:{1:00}", span.Minutes, span.Seconds);
    }

}
