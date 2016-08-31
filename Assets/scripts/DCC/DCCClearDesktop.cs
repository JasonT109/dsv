using UnityEngine;

public class DCCClearDesktop : MonoBehaviour
{
    public buttonGroup clearGroup;
    public buttonControl clearButton;

	void Update ()
    {
	    if (clearButton.pressed)
        {
            clearGroup.toggleAllOff();
        }
	}
}
