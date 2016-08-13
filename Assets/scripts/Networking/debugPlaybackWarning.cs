using UnityEngine;
using System.Collections;
using Meg.EventSystem;

public class debugPlaybackWarning : MonoBehaviour
{

    public buttonControl Button;

	private void Start()
	{
	    if (!Button)
	        Button = GetComponent<buttonControl>();
	}
	
	private void Update()
	{
	    Button.warning = megEventManager.Instance.Playing;
	}
}
