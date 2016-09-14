using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class debugDCCDisplayUi : MonoBehaviour
{

    public debugDCCScreensUi Screens;
    public debugDCCStationsUi Stations;

    public Graphic ScreensOn;
    public Graphic StationsOn;


    private void Update()
    {
        ScreensOn.gameObject.SetActive(Screens.gameObject.activeSelf);
        StationsOn.gameObject.SetActive(Stations.gameObject.activeSelf);
    }

    public void SelectScreens()
    {
        Screens.gameObject.SetActive(true);
        Stations.gameObject.SetActive(false);
    }

    public void SelectStations()
    {
        Screens.gameObject.SetActive(false);
        Stations.gameObject.SetActive(true);
    }

}
