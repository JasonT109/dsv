using UnityEngine;
using System.Collections;
using Meg.EventSystem;

public class debugPlaybackWarning : MonoBehaviour
{

    public buttonControl Button;

    public Renderer PlayIcon;
    public Renderer PauseIcon;

    private void Start()
	{
	    if (!Button)
	        Button = GetComponent<buttonControl>();

        Update();
	}
	
	private void Update()
	{
        var playing = megEventManager.Instance.Playing;

	    Button.warning = playing;

	    if (PlayIcon)
	        PlayIcon.enabled = !playing;
        if (PauseIcon)
            PauseIcon.enabled = playing;
    }
}
