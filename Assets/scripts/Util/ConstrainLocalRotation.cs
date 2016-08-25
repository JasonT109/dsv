using UnityEngine;
using System.Collections;

public class ConstrainLocalRotation : MonoBehaviour 
{

    public bool constrainRX = false;
    public bool constrainRY = false;
    public bool constrainRZ = false;

    private float rx;
    private float ry;
    private float rz;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
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

        Vector3 constrained = new Vector3(transform.localRotation.eulerAngles.x, 0f, transform.localRotation.eulerAngles.x);
        transform.localRotation = Quaternion.Euler(constrained);
	}
}
