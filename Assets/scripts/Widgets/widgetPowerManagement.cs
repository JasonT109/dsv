using UnityEngine;
using System.Collections;

public class widgetPowerManagement : MonoBehaviour
{

    public buttonControl[] buttons;
    public widgetPowerGroup[] powerGroups;

	void Update ()
    {
	    for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].active && powerGroups[i].powerOn)
            {
                powerGroups[i].powerOn = false;
            }
            else if (buttons[i].active && !powerGroups[i].powerOn)
            {
                powerGroups[i].powerOn = true;
            }
        }
	}
}
