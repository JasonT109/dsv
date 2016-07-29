using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class AnimateOnClickUi : MonoBehaviour
{
    public Button Button;
    public Vector2 Strength = Vector2.one * 0.1f;
    public float Duration = 0.25f;

	private void Start()
	{
	    if (!Button)
	        Button = GetComponent<Button>();

        if (Button)
            Button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (Button)
            Button.transform.DOPunchScale(Strength, Duration);
    }
}
