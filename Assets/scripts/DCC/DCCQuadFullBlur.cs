using UnityEngine;
using System.Collections;

public class DCCQuadFullBlur : MonoBehaviour
{
    public GameObject blurObject;

    public DCCScreenManager manager;

	void Start ()
    {
        if (!manager)
            manager = ObjectFinder.Find<DCCScreenManager>();
	}

	void Update ()
    {
        if (manager && blurObject)
        {
            for (int i = 0; i < manager.quadBoxes.Length; i++)
            {
                if (manager.quadBoxes[i].boxPosition == Meg.DCC.DCCScreenContentPositions.positionID.middle)
                {
                    if (manager.quadBoxes[i].boxContent != DCCWindow.contentID.none)
                        blurObject.SetActive(true);
                    else
                        blurObject.SetActive(false);
                }
            }
        }
	}
}
