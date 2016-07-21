using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetHeadingDial : MonoBehaviour
{
    public bool reverse = false;
    public bool flat = false;
    public bool onYAxis = false;
    private Quaternion q;

    float x = 0;
    // Update is called once per frame
    void Update ()
    {
        if (onYAxis)
        {
            //transform.Rotate(0, 0, serverUtils.GetServerData("heading"), Space.Self);
            //q = Quaternion.AngleAxis(serverUtils.GetServerData("heading"), transform.forward);
            q = Quaternion.Euler(new Vector3(transform.localEulerAngles.x, 0, -serverUtils.GetServerData("heading")));

            transform.localRotation = q;
        }
        else
        {
            if (flat)
            {
                x = -90f;
            }

            if (reverse)
            {
                q = Quaternion.Euler(new Vector3(x, 180, -serverUtils.GetServerData("heading")));
            }
            else
            {
                q = Quaternion.Euler(new Vector3(x, 180, serverUtils.GetServerData("heading")));
            }
            transform.rotation = q;
        }
    }
}
