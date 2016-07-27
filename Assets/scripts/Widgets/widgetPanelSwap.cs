using UnityEngine;
using System.Collections;

public class widgetPanelSwap : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public buttonControl swapButton;
    public bool changed = false;
    public bool canPress = true;

    IEnumerator wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPress = true;
    }

    void Update ()
    {
        if (swapButton.pressed && canPress)
        {
            changed = true;
            canPress = false;
            StartCoroutine(wait(0.1f));
        }

	    if (panel1.activeInHierarchy && changed)
        {
            Debug.Log("Setting panel 2 active.");
            changed = false;
            panel1.SetActive(false);
            panel2.SetActive(true);
        }

        if (panel2.activeInHierarchy && changed)
        {
            Debug.Log("Setting panel 1 active.");
            changed = false;
            panel1.SetActive(true);
            panel2.SetActive(false);
        }
	}
}
