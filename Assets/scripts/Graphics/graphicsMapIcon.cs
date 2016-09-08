using UnityEngine;
using System.Collections;
using Meg.Networking;

public class graphicsMapIcon : MonoBehaviour {

    public GameObject button;
    public GameObject directionMarker;
    public graphicsSlicedMesh textBackdrop; 

    public int direction; //0 no direction, 1 left, 2 upleft, 3 up, 4 up right, 5 right, 6 down right, 7 down, 8 down left
    public bool atBounds = false;
    public TextMesh label;

    public int VesselId { get; set; }

    private Renderer _textRenderer;
    private float _lastTextWidth;

    public void UpdateIcon(bool isAtBounds, int iconDirection)
    {
        if (!_textRenderer)
            _textRenderer = label.GetComponent<Renderer>();

        label.text = serverUtils.GetVesselName(VesselId).ToUpper();
        var bounds = _textRenderer.bounds;
        var textWidth = bounds.size.x * 0.3f;

        if (!textBackdrop)
            textBackdrop = label.GetComponentInChildren<graphicsSlicedMesh>();

        if (!textBackdrop)
            return;

        if (!Mathf.Approximately(_lastTextWidth, textWidth))
            textBackdrop.Width = textWidth + 0.1f;

        _lastTextWidth = textWidth;

        var labelX = -(0.75f + (textWidth * 0.5f));
        label.transform.localPosition = new Vector3(labelX, 0, 0);

        atBounds = isAtBounds;
        direction = iconDirection;

        var mapPinManager = ObjectFinder.Find<NavSubPins>();
        if (mapPinManager.isGliderMap)
            transform.localScale = new Vector3(0.27f, transform.localScale.y, transform.localScale.z);

        if (atBounds)
        {
            button.GetComponent<Renderer>().enabled = false;
            directionMarker.GetComponent<Renderer>().enabled = true;

            switch (direction)
            {
                case 0:
                    //0 no direction
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -180);
                    break;
                case 1:
                    //1 left
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -180);
                    break;
                case 2:
                    //2 upleft
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -135);
                    break;
                case 3:
                    //3 up
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -90);
                    break;
                case 4:
                    //4 up right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, -45);
                    label.transform.localPosition = new Vector3(-labelX, 0, 0);
                    break;
                case 5:
                    //5 right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 0);
                    label.transform.localPosition = new Vector3(-labelX, 0, 0);
                    break;
                case 6:
                    //6 down right
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 45);
                    label.transform.localPosition = new Vector3(-labelX, 0, 0);
                    break;
                case 7:
                    //7 down
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 90);
                    break;
                case 8:
                    //8 down left
                    directionMarker.transform.rotation = Quaternion.Euler(0, 180, 135);
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
