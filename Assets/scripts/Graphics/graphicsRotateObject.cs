using UnityEngine;

public class graphicsRotateObject : MonoBehaviour
{

    public widgetThrusterControl thrusterControl;
    public bool mainL;
    public bool mainR;
    public bool sideL1;
    public bool sideL2;
    public bool sideL3;
    public bool sideR1;
    public bool sideR2;
    public bool sideR3;
    public bool mainLVector;
    public bool mainRVector;
    public float multiplier = 1.0f;
    private float value = 0f;
    public int axis = 0;
    public bool local;

    void Start ()
    {
        thrusterControl = ObjectFinder.Find<widgetThrusterControl>();
    }

	void Update ()
    {
        if (mainL)
        {
            value = thrusterControl.thrusterMainL;
        }
        if (mainR)
        {
            value = thrusterControl.thrusterMainR;
        }
        if (sideL1)
        {
            value = thrusterControl.thrusterSideL1;
        }
        if (sideL2)
        {
            value = thrusterControl.thrusterSideL2;
        }
        if (sideL3)
        {
            value = thrusterControl.thrusterSideL3;
        }
        if (sideR1)
        {
            value = thrusterControl.thrusterSideR1;
        }
        if (sideR2)
        {
            value = thrusterControl.thrusterSideR2;
        }
        if (sideR3)
        {
            value = thrusterControl.thrusterSideR3;
        }
        if (mainLVector)
        {
            value = thrusterControl.thrusterVectorAngleL;
        }
        if (mainRVector)
        {
            value = thrusterControl.thrusterVectorAngleR;
        }

        if (mainLVector)
        {
            if (local)
                gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, Quaternion.Euler(0, -value * 0.01f, 0), Time.deltaTime * multiplier);
            else
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, -value * 0.01f, 0), Time.deltaTime * multiplier);
        }
        else if (mainRVector)
        {
            if (local)
                gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, Quaternion.Euler(0, -value * 0.01f, 0), Time.deltaTime * multiplier);
            else
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, -value * 0.01f, 0), Time.deltaTime * multiplier);
        }
        else
        {
            switch (axis)
            {
                case 0:
                    gameObject.transform.Rotate(value * multiplier, 0, 0);
                    break;
                case 1:
                    gameObject.transform.Rotate(0, value * multiplier, 0);
                    break;
                case 2:
                    gameObject.transform.Rotate(0, 0, value * multiplier);
                    break;
            }
        }
    }
}
