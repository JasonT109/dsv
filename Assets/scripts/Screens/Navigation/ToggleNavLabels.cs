using UnityEngine;
using System.Collections;

public class ToggleNavLabels : MonoBehaviour
{

    public buttonControl Button;

	private void Start()
	{
	    if (!Button)
	        Button = GetComponent<buttonControl>();

	    if (!Button)
	        return;

	    Button.onPressed += OnButtonPressed;
	}

    private void OnButtonPressed()
    {
        NavSubPins.Instance.ToggleLabels();
    }
}
