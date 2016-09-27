using System;
using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class Window : MonoBehaviour
{

    private PressGesture _pressGesture;

	private void Start()
	{
        _pressGesture = GetComponentInChildren<PressGesture>();
        _pressGesture.Pressed += OnPressed;
	}

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }

    private void OnPressed(object sender, EventArgs e)
    {
        transform.SetAsLastSibling();
    }

}
