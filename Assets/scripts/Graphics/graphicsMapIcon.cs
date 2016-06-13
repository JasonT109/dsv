using UnityEngine;
using System.Collections;

public class graphicsMapIcon : MonoBehaviour {

    public GameObject button;
    public GameObject directionMarker;
    public int direction; //0 no direction, 1 left, 2 upleft, 3 up, 4 up right, 5 right, 6 down right, 7 down, 8 down left
    public bool atBounds = false;
    public TextMesh label;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (atBounds)
        {
            button.GetComponent<Renderer>().enabled = false;
            directionMarker.GetComponent<Renderer>().enabled = true;

            switch (direction)
            {
                case 0:
                    //0 no direction
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -180);
                    label.anchor = TextAnchor.LowerLeft;
                    label.transform.localPosition = new Vector3(0.62f, 0.13f, 0.4f);
                    break;
                case 1:
                    //1 left
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -180);
                    label.anchor = TextAnchor.LowerLeft;
                    label.transform.localPosition = new Vector3(0.62f, 0.13f, 0.4f);
                    break;
                case 2:
                    //2 upleft
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -135);
                    label.anchor = TextAnchor.UpperLeft;
                    label.transform.localPosition = new Vector3(0.18f, -0.07f, 0.4f);
                    break;
                case 3:
                    //3 up
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -90);
                    label.transform.localPosition = new Vector3(0.18f, -0.07f, 0.4f);
                    label.anchor = TextAnchor.UpperLeft;
                    break;
                case 4:
                    //4 up right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -45);
                    label.transform.localPosition = new Vector3(0.095f, 0.41f, 0.4f);
                    label.anchor = TextAnchor.UpperRight;
                    break;
                case 5:
                    //5 right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 0);
                    label.transform.localPosition = new Vector3(0.22f, 0.53f, 0.4f);
                    label.anchor = TextAnchor.LowerRight;
                    break;
                case 6:
                    //6 down right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 45);
                    label.transform.localPosition = new Vector3(0.22f, 0.53f, 0.4f);
                    label.anchor = TextAnchor.LowerRight;
                    break;
                case 7:
                    //7 down
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 90);
                    label.transform.localPosition = new Vector3(0.62f, 0.13f, 0.4f);
                    label.anchor = TextAnchor.LowerLeft;
                    break;
                case 8:
                    //8 down left
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 135);
                    label.transform.localPosition = new Vector3(0.62f, 0.13f, 0.4f);
                    label.anchor = TextAnchor.LowerLeft;
                    break;
            }
        }
        else
        {
            button.GetComponent<Renderer>().enabled = true;
            directionMarker.GetComponent<Renderer>().enabled = false;
        }
	}
}
