using UnityEngine;
using System.Collections;
using Meg.Maths;
using Meg.Networking;

public class graphicsGliderRotate : MonoBehaviour {

    public bool yaw = false;
    public bool pitch = false;
    public bool roll = false;

    public float yawMin = 0f;
    public float yawMid = 45f;
    public float yawMax = 90f;

    public float pitchMin = 0f;
    public float pitchMid = 45f;
    public float pitchMax = 90f;

    public float rollMin = 0f;
    public float rollMid = 45f;
    public float rollMax = 90f;

    public string serverDrivingParam = "pitch";
    public float serverMin = 0f;
    public float serverMax = 90f;

    private float pitchValue = 0;
    private float yawValue = 0;
    private float rollValue = 0;

    void Start()
    {
        pitchValue = gameObject.transform.localEulerAngles.x;
        yawValue = gameObject.transform.localEulerAngles.y;
        rollValue = gameObject.transform.localEulerAngles.z;
    }

	// Update is called once per frame
	void Update ()
    {
        float blendAmount = graphicsMaths.remapValue(Mathf.Clamp( serverUtils.GetServerData(serverDrivingParam), serverMin, serverMax ), serverMin, serverMax, 0, 1);
        //blendAmount = graphicsEasing.EaseInOut(blendAmount, EasingType.Quadratic);

        if (roll)
        {
            if (blendAmount > 0.5f)
            {
                rollValue = graphicsMaths.remapValue(blendAmount, 0.5f, 1, rollMid, rollMax);
            }
            else
            {
                rollValue = graphicsMaths.remapValue(blendAmount, 0, 0.5f, rollMin, rollMid);
            }
        }
        if (pitch)
        {
            if (blendAmount > 0.5f)
            {
                pitchValue = graphicsMaths.remapValue(blendAmount, 0.5f, 1, pitchMid, pitchMax);
            }
            else
            {
                pitchValue = graphicsMaths.remapValue(blendAmount, 0, 0.5f, pitchMin, pitchMid);
            }

        }
        if (yaw)
        {
            if (blendAmount > 0.5f)
            {
                yawValue = graphicsMaths.remapValue(blendAmount, 0.5f, 1, yawMid, yawMax);
            }
            else
            {
                yawValue = graphicsMaths.remapValue(blendAmount, 0, 0.5f, yawMin, yawMid);
            }
        }

        Quaternion combinedRotation = Quaternion.Euler(new Vector3(pitchValue, yawValue, rollValue));

        gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, combinedRotation, Time.deltaTime);
    }
}
