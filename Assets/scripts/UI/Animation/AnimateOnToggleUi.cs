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
    public bool AnimateWhenTurningOff = false;

	private void Start()
	{
	    if (!Toggle)
            Toggle = GetComponent<Toggle>();

        if (Toggle)
            Toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool value)
    {
        if (!value && !AnimateWhenTurningOff)
            return;

        if (!Toggle || DOTween.IsTweening(Toggle.transform))
            return;

        Toggle.transform.DOPunchScale(Strength, Duration);
    }
}
