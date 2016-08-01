using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetGliderVesselVisUI : MonoBehaviour
{
    public GameObject[] vesselButtons;
    public bool[] vesselVisibility = new bool[5];

    void Start()
    {
        vesselButtons = new GameObject[transform.childCount];
        for (int i = 0; i < vesselVisibility.Length; i++)
        {
            vesselVisibility[i] = serverUtils.GetVesselVis(i + 1);
            vesselButtons[i] = transform.GetChild(i).gameObject;
            vesselButtons[i].SetActive(vesselVisibility[i]);
        }
    }

    void Update ()
    {
        float yPos = 0;

        for (int i = 0; i < vesselVisibility.Length; i++)
        {
            if (serverUtils.GetVesselVis(i + 1))
            {
                vesselVisibility[i] = true;
                vesselButtons[i].SetActive(true);
                vesselButtons[i].transform.localPosition = new Vector3(0, yPos, 0);
                yPos -= 1;
            }
            else
            {
                vesselVisibility[i] = false;
                vesselButtons[i].SetActive(false);
            }
        }
    }
}
