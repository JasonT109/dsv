using UnityEngine;
using System.Collections;

public class graphicsMapIcon : MonoBehaviour {

    public GameObject button;
    public GameObject directionMarker;
    public int direction; //0 no direction, 1 left, 2 upleft, 3 up, 4 up right, 5 right, 6 down right, 7 down, 8 down left
    public bool useAnchor = true;
    public bool atBounds = false;
    public TextMesh label;
    public Vector3 lowerLeftPos = new Vector3(0.62f, 0.13f, 0.4f);
    public Vector3 leftPos = new Vector3(0.62f, 0.13f, 0.4f);
    public Vector3 upperLeftPos = new Vector3(0.18f, -0.07f, 0.4f);
    public Vector3 upPos = new Vector3(0.18f, -0.07f, 0.4f);
    public Vector3 upperRightPos = new Vector3(0.095f, 0.41f, 0.4f);
    public Vector3 rightPos = new Vector3(0.22f, 0.53f, 0.4f);
    public Vector3 lowerRightPos = new Vector3(0.22f, 0.53f, 0.4f);
    public Vector3 lowPos = new Vector3(0.62f, 0.13f, 0.4f);

    
    public void UpdateIcon(bool isAtBounds, int iconDirection)
    {
        atBounds = isAtBounds;
        direction = iconDirection;

        if (atBounds)
        {
            button.GetComponent<Renderer>().enabled = false;
            directionMarker.GetComponent<Renderer>().enabled = true;

            switch (direction)
            {
                case 0:
                    //0 no direction
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -180);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.MiddleLeft;
                    }
                    label.transform.localPosition = leftPos;
                    break;
                case 1:
                    //1 left
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -180);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.MiddleLeft;
                    }
                    label.transform.localPosition = leftPos;
                    break;
                case 2:
                    //2 upleft
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -135);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.UpperLeft;
                    }
                    label.transform.localPosition = upperLeftPos;
                    break;
                case 3:
                    //3 up
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -90);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.UpperLeft;
                    }
                    if (transform.localPosition.x < 0)
                        label.transform.localPosition = upperLeftPos;
                    else
                        label.transform.localPosition = upperRightPos;
                    break;
                case 4:
                    //4 up right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -45);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.UpperRight;
                    }
                    label.transform.localPosition = upperRightPos;
                    break;
                case 5:
                    //5 right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 0);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.MiddleRight;
                    }
                    label.transform.localPosition = rightPos;
                    break;
                case 6:
                    //6 down right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 45);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.LowerRight;
                    }
                    label.transform.localPosition = lowerRightPos;
                    break;
                case 7:
                    //7 down
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 90);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.LowerLeft;
                    }
                    if (transform.localPosition.x < 0)
                        label.transform.localPosition = lowerLeftPos;
                    else
                        label.transform.localPosition = lowerRightPos;
                    break;
                case 8:
                    //8 down left
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 135);
                    if (useAnchor)
                    {
                        label.anchor = TextAnchor.LowerLeft;
                    }
                    label.transform.localPosition = lowerLeftPos;
                    break;
            }
        }
        else
        {
            button.GetComponent<Renderer>().enabled = true;
            directionMarker.GetComponent<Renderer>().enabled = false;

            if (transform.localPosition.x > 0)
            {
                if (useAnchor)
                {
                    label.anchor = TextAnchor.MiddleRight;
                }
                label.transform.localPosition = rightPos;
            }
            else
            {
                if (useAnchor)
                {
                    label.anchor = TextAnchor.MiddleLeft;
                }
                label.transform.localPosition = leftPos;
            }
        }
	}
}
