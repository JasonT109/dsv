using UnityEngine;
using System.Collections;

public class graphicsTileObject : MonoBehaviour
{
    public float xOffset = 0;
    public float zOffset = 0;
    public float distanceSnap = 100f;
    public GameObject followObject;
    public bool constrainYTranslate = true;
    private Vector3 prevPos = new Vector3(0, 0, 1);
    public Vector3 travelDirection = new Vector3(0, 0, 1);
    public float x = 0f;
    public float z = 0f;
    public float xDistance;
    public float zDistance;
    private bool canUpdate = false;

    void Start()
    {
        StartCoroutine(wait(0.1f));
    }

    void Update ()
    {
        if (followObject && canUpdate)
        {
            xDistance = Vector3.Distance(new Vector3(followObject.transform.position.x, 0, 0), new Vector3(transform.position.x, 0, 0));
            zDistance = Vector3.Distance(new Vector3(0, 0, followObject.transform.position.z), new Vector3(0, 0, transform.position.z));
            travelDirection = (followObject.transform.position - prevPos);
            if (travelDirection.x > 0)
            {
                x = 1f;
            }
            else if (travelDirection.x < 0)
            {
                x = -1f;
            }
            else
            {
                x = 0f;
            }
            if (travelDirection.z > 0)
            {
                z = 1f;
            }
            else if (travelDirection.z < 0)
            {
                z = -1f;
            }
            else
            {
                z = 0f;
            }
            
            if (xDistance > distanceSnap)
            {
                transform.position = new Vector3(transform.position.x + (xOffset * x), transform.position.y, transform.position.z);
            }
            if (zDistance > distanceSnap)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (zOffset * z));
            }
            if (constrainYTranslate)
            {
                transform.position = new Vector3(transform.position.x, followObject.transform.position.y, transform.position.z);
            }
            prevPos = followObject.transform.position;
        }
    }

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canUpdate = true;
    }
}
