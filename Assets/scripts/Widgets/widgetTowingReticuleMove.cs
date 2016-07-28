using UnityEngine;
using System.Collections;

public class widgetTowingReticuleMove : MonoBehaviour {

    public Vector2 moveDir = Vector2.zero;
    public float maxX = 6;
    public float maxY = 5;
    public float moveSpeed = 1;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey("[8]"))
            moveDir = new Vector2(0, 1);
        else if (Input.GetKey("[9]"))
            moveDir = new Vector2(1, 1);
        else if (Input.GetKey("[6]"))
            moveDir = new Vector2(1, 0);
        else if (Input.GetKey("[3]"))
            moveDir = new Vector2(1, -1);
        else if (Input.GetKey("[2]"))
            moveDir = new Vector2(0, -1);
        else if (Input.GetKey("[1]"))
            moveDir = new Vector2(-1, -1);
        else if (Input.GetKey("[4]"))
            moveDir = new Vector2(-1, 0);
        else if (Input.GetKey("[7]"))
            moveDir = new Vector2(-1, 1);
        else
            moveDir = new Vector2(0, 0);

        gameObject.transform.Translate(moveDir * moveSpeed);

    }
}
