using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class AnimateOnToggleUi : MonoBehaviour
{
    public Toggle Toggle;
    public Vector2 Strength = Vector2.one * 0.1f;
    public float Duration = 0.25f;

	private void Start()
	{
	    if (!Toggle)
            Toggle = GetComponent<Toggle>();

        if (Toggle)
            Toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool value)
    {
        if (Toggle)
            Toggle.transform.DOPunchScale(Strength, Duration);
    }
}
