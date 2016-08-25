using UnityEngine;
using System.Collections;

public class graphicsSetLinePositions : MonoBehaviour {

    public Transform targetPos;
    public LineRenderer line;

    private Vector3[] positions = new Vector3[2];

	// Use this for initialization
	void Start () {
        if (!line)
            line = gameObject.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        positions[0] = transform.position;
        positions[1] = targetPos.position;

        line.SetPositions(positions);
	}
}
