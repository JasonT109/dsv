using UnityEngine;
using System.Collections;
using Meg.Networking;

public class ToggleNavAutoPilot : MonoBehaviour
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

    private void Update()
    {
        Button.active = serverUtils.GetServerBool("isAutoPilot");
    }

    private void OnButtonPressed()
    {
        var active = !serverUtils.GetServerBool("isAutoPilot");
        serverUtils.PostServerData("isAutoPilot", active ? 1 : 0);
    }
}
