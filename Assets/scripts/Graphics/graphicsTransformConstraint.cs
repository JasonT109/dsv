using UnityEngine;
using System.Collections;

public class graphicsTransformConstraint : MonoBehaviour
{
    public Transform constrainTo;
    public bool PositionX;
    public bool PositionY;
    public bool PositionZ;

    // private Vector3 initPosition;
    private Vector3 newPosition;

    // Use this for initialization
    void Start ()
    {
        // initPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        newPosition = transform.position;

	    if (PositionX)
        {
            newPosition.x = constrainTo.position.x;    
        }
        if (PositionY)
        {
            newPosition.y = constrainTo.position.y;
        }
        if (PositionZ)
        {
            newPosition.z = constrainTo.position.z;
        }

        transform.position = newPosition;
    }
}
