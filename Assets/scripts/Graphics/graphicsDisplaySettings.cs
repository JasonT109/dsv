using UnityEngine;
using System.Collections;

public class graphicsDisplaySettings : MonoBehaviour {

    public GameObject b16x9;
    public GameObject b16x10;
    public GameObject b21x9l;
    public GameObject b21x9c;
    public GameObject b21x9r;
    public float offset_16x9_X = 0f;
    public float offset_16x10_X = 0f;
    public float offset_21x9l_X = -41.1f;
    public float offset_21x9c_X = 0f;
    public float offset_21x9r_X = 41.1f;
    public float leftLargePanelXOffset = -83.6f;
    public GameObject mainPanel;
    public GameObject panelLeftSmall;
    public GameObject panelRightSmall;
    public GameObject panelLeftLarge;
    public GameObject panelRightLarge;

    // Update is called once per frame
    void Update ()
    {
	    if (b16x9.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_16x9_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(false);
        }
        if (b16x10.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_16x10_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(false);
        }
        if (b21x9l.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9l_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(true);
        }
        if (b21x9c.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9c_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(true);
            panelRightSmall.SetActive(true);
            panelLeftLarge.SetActive(false);
            panelRightLarge.SetActive(false);
        }
        if (b21x9r.GetComponent<buttonControl>().active)
        {
            mainPanel.transform.localPosition = new Vector3(offset_21x9r_X, mainPanel.transform.localPosition.y, mainPanel.transform.localPosition.z);
            panelLeftSmall.SetActive(false);
            panelRightSmall.SetActive(false);
            panelLeftLarge.SetActive(true);
            panelRightLarge.SetActive(false);
            panelLeftLarge.transform.localPosition = new Vector3(leftLargePanelXOffset, panelLeftLarge.transform.localPosition.y, panelLeftLarge.transform.localPosition.z);
}
    }
}
