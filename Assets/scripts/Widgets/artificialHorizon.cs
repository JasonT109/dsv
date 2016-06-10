using UnityEngine;
using System.Collections;
using Meg.Networking;

public class artificialHorizon : MonoBehaviour {

    public GameObject optionalHorizon;
    public bool isHorizonBall;
    public float horizonMaxPosition = 30f;
    public float horizonMinPosition = -30f;
    private Quaternion q;
    private float degreeDistance;

    void Start()
    {
        //localPosition distance for 1 degree
        degreeDistance = (horizonMaxPosition - horizonMinPosition) / 360;
    }

    // Update is called once per frame
    void Update ()
    {
        if (optionalHorizon)
        {
            if (isHorizonBall)
            {
                Quaternion h = Quaternion.Euler(new Vector3(serverUtils.GetServerData("rollAngle"), 90, (serverUtils.GetServerData("pitchAngle") * 2f)));
                optionalHorizon.transform.rotation = h;
            }
            else
            {
                optionalHorizon.transform.localPosition = new Vector3(0, (serverUtils.GetServerData("pitchAngle") * degreeDistance), optionalHorizon.transform.localPosition.z);
            }
        }
        q = Quaternion.Euler(new Vector3(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, -serverUtils.GetServerData("rollAngle")));
        transform.rotation = q;
    }
}
