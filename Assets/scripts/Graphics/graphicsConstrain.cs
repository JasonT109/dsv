using UnityEngine;
using System.Collections;

public class graphicsConstrain : MonoBehaviour {

    public bool constrainRX = false;
    public bool constrainRY = false;
    public bool constrainRZ = false;

    private float rx;
    private float ry;
    private float rz;

    // Update is called once per frame
    void Update () {

        rx = transform.rotation.eulerAngles.x;
        ry = transform.rotation.eulerAngles.y;
        rz = transform.rotation.eulerAngles.z;

        if (constrainRX)
        {
            rx = 0;
        }
        if (constrainRY)
        {
            ry = 0;
        }
        if (constrainRZ)
        {
            rz = 0;
        }

        transform.rotation = Quaternion.Euler(rx, ry, rz);
    }
}
