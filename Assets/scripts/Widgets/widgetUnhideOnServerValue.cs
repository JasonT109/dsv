using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetUnhideOnServerValue : MonoBehaviour
{

    public string serverString = "genericerror";
    public float serverValue = 0;
    public bool greaterThanValue = false;
    public GameObject[] objectsToUnhide;
	
    void SetVisibility()
    {
        bool visible = false;

        if (greaterThanValue)
        {
            if (serverUtils.GetServerData(serverString) > serverValue)
            {
                visible = true;
            }
            else
            {
                visible = false;
            }
        }
        else
        {
            if (serverUtils.GetServerData(serverString) <= serverValue)
            {
                visible = true;
            }
            else
            {
                visible = false;
            }
        }

        for (int i = 0; i < objectsToUnhide.Length; i++)
        {
            objectsToUnhide[i].SetActive(visible);
        }
    }

    void OnEnable()
    {
        SetVisibility();
    }

    void Start ()
    {
        SetVisibility();
    }

	// Update is called once per frame
	void Update ()
    {
        SetVisibility();

    }
}
