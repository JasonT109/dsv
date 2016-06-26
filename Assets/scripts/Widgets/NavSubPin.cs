using UnityEngine;
using System.Collections;

public class NavSubPin : MonoBehaviour 
{
    crewData VesselData;
    public GameObject Vessel1;
    public GameObject Vessel2;
    public GameObject Vessel3;
    public GameObject Vessel4;

    public float MapSize;

    public GameObject ServerData;

	// Use this for initialization
	void Start () 
    {
        VesselData = ServerData.GetComponent<crewData>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 Vessel = VesselData.vessel1Pos;
        Vessel1.transform.localPosition = ConvertVesselCoords(Vessel);

        Vessel = VesselData.vessel2Pos;
        Vessel2.transform.localPosition = ConvertVesselCoords(Vessel);

        Vessel = VesselData.vessel3Pos;
        Vessel3.transform.localPosition = ConvertVesselCoords(Vessel);

        Vessel = VesselData.vessel4Pos;
        Vessel4.transform.localPosition = ConvertVesselCoords(Vessel);


	}

    Vector3 ConvertVesselCoords(Vector3 _Vessel)
    {
        Vector3 TempVessel;

        //"normalise" the coordinate system used in the crew data map
        //hardcoded with the size of the crew map size
        TempVessel.x = (_Vessel.x + 5.0f) / 10.0f;
        TempVessel.y = 1.0f;
        TempVessel.z = (_Vessel.y + 5.0f) / 10.0f;

        //convert the normalised coordinates to map to the nav map
        //hardcoded with a displacement to the nav map
        TempVessel.x = TempVessel.x * MapSize + 130.0f;
        TempVessel.z = TempVessel.z * MapSize + -25.0f;

        return(TempVessel);
    }
}
