using UnityEngine;
using System.Collections;

public class NavSubPin : MonoBehaviour 
{
    crewData VesselData;
    public GameObject Vessel1;
    public GameObject Vessel2;
    public GameObject Vessel3;
    public GameObject Vessel4;
    public float seaFloor = 10994f;
    public Camera mapCamera;
    public float MapSize;
    public Vector2 imageSize = new Vector2(5, 5);
    public GameObject ServerData;

    private Vector3[] vesselMapSource = new Vector3[4];
    public GameObject[] vesselButtons = new GameObject[4];
    public GameObject[] vessels = new GameObject[4];

    // Use this for initialization
    void Start () 
    {
        VesselData = ServerData.GetComponent<crewData>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < vesselButtons.Length; i++)
        {
            //vessel data
            Vector3 Vessel = new Vector3();

            //0 no direction, 1 left, 2 upleft, 3 up, 4 up right, 5 right, 6 down right, 7 down, 8 down left
            int mapIconDirection = 0;

            //get vessel position from crew data
            switch (i)
            {
                case 0:
                    Vessel = VesselData.vessel1Pos;
                    vessels[i].transform.localPosition = ConvertVesselCoords(Vessel);
                    break;
                case 1:
                    Vessel = VesselData.vessel2Pos;
                    vessels[i].transform.localPosition = ConvertVesselCoords(Vessel);
                    break;
                case 2:
                    Vessel = VesselData.vessel3Pos;
                    vessels[i].transform.localPosition = ConvertVesselCoords(Vessel);
                    break;
                case 3:
                    Vessel = VesselData.vessel4Pos;
                    vessels[i].transform.localPosition = ConvertVesselCoords(Vessel);
                    break;
            }

            //get position in map space
            vesselMapSource[i] = ConvertToMapSpace(vessels[i]);

            //set the position
            vesselButtons[i].transform.localPosition = vesselMapSource[i];

            //check to see if child is at edge of the map
            if (vesselButtons[i].transform.localPosition.x == -imageSize.x && vesselButtons[i].transform.localPosition.y == -imageSize.y)
            {
                mapIconDirection = 8;
            }
            else if (vesselButtons[i].transform.localPosition.x == -imageSize.x && vesselButtons[i].transform.localPosition.y == imageSize.y)
            {
                mapIconDirection = 2;
            }
            else if (vesselButtons[i].transform.localPosition.x == -imageSize.x)
            {
                mapIconDirection = 1;
            }

            if (vesselButtons[i].transform.localPosition.x == imageSize.x && vesselButtons[i].transform.localPosition.y == -imageSize.y)
            {
                mapIconDirection = 6;
            }
            else if (vesselButtons[i].transform.localPosition.x == imageSize.x && vesselButtons[i].transform.localPosition.y == imageSize.y)
            {
                mapIconDirection = 4;
            }
            else if (vesselButtons[i].transform.localPosition.x == imageSize.x)
            {
                mapIconDirection = 5;
            }

            if (vesselButtons[i].transform.localPosition.y == imageSize.y && vesselButtons[i].transform.localPosition.x != imageSize.x && vesselButtons[i].transform.localPosition.x != -imageSize.x)
            {
                mapIconDirection = 3;
            }
            if (vesselButtons[i].transform.localPosition.y == -imageSize.y && vesselButtons[i].transform.localPosition.x != imageSize.x && vesselButtons[i].transform.localPosition.x != -imageSize.x)
            {
                mapIconDirection = 7;
            }

            //set the orientation of the child to indicate the direction
            if (mapIconDirection != 0)
            {
                vesselButtons[i].GetComponent<graphicsMapIcon>().atBounds = true;
                vesselButtons[i].GetComponent<graphicsMapIcon>().direction = mapIconDirection;
            }
            else
            {
                vesselButtons[i].GetComponent<graphicsMapIcon>().atBounds = false;
                vesselButtons[i].GetComponent<graphicsMapIcon>().direction = mapIconDirection;
            }
        }
    }

    Vector2 ConvertToMapSpace(GameObject _Vessel)
    {
        Vector2 TempMapPos;

        Vector3 c = mapCamera.WorldToViewportPoint(_Vessel.transform.position);
        TempMapPos.x = Mathf.Clamp((c.x * 10.0f) - 5.0f, -imageSize.x, imageSize.x);
        TempMapPos.y = Mathf.Clamp((c.y * 10.0f) - 5.0f, -imageSize.y, imageSize.y);

        return TempMapPos;
    }

    Vector3 ConvertVesselCoords(Vector3 _Vessel)
    {
        Vector3 TempVessel;

        //"normalise" the coordinate system used in the crew data map
        //hardcoded with the size of the crew map size
        TempVessel.x = (_Vessel.x + 5.0f) / 10.0f;
        TempVessel.y = (seaFloor - _Vessel.z) / 1000.0f;
        TempVessel.z = (_Vessel.y + 5.0f) / 10.0f;

        //convert the normalised coordinates to map to the nav map
        //hardcoded with a displacement to the nav map
        TempVessel.x = TempVessel.x * MapSize + 130.0f;
        TempVessel.z = TempVessel.z * MapSize + -25.0f;

        return(TempVessel);
    }
}
