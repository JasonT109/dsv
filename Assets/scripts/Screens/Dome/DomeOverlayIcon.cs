using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using TouchScript.Gestures;

public class DomeOverlayIcon : MonoBehaviour
{
    public const string ScreenTag = "DomeScreen";

    [Header("Components")]
    public buttonControl Button;

    [Header("Configuration")]
    public DomeScreen.Overlay Overlay;

    private Vector3 _homePosition;
    private bool _pressed;

    private DomeScreen _hovered;

    private DomeScreen Hovered
    {
        get { return _hovered; }
        set
        {
            if (_hovered == value)
                return;

            if (_hovered)
                _hovered.Hovering = false;

            _hovered = value;

            if (_hovered)
                _hovered.Hovering = true;
        }
    }


    private void Awake()
    {
        _homePosition = transform.position;

        if (!Button)
            Button = GetComponent<buttonControl>();
    }

    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += OnTapped;
        GetComponent<PressGesture>().Pressed += OnPressed;
        GetComponent<ReleaseGesture>().Released += OnReleased;
    }

    private void OnDisable()
    {
        GetComponent<TapGesture>().Tapped -= OnTapped;
        GetComponent<PressGesture>().Pressed -= OnPressed;
        GetComponent<ReleaseGesture>().Released -= OnReleased;
    }

    private void Update()
    {
        if (_pressed)
            Hovered = GetScreenUnderIcon();
    }

    private void OnTapped(object sender, EventArgs e)
    {
    }

    private void OnPressed(object sender, EventArgs e)
    {
        _pressed = true;
    }

    private void OnReleased(object sender, EventArgs e)
    {
        // Drop overlay onto hovered screen.
        if (Hovered)
        {
            Hovered.On = true;
            Hovered.Current = Overlay;
        }

        // Restore icon to original position.
        if (Hovered)
            StartCoroutine(ReleaseOverScreenRoutine());
        else
            StartCoroutine(ReleaseOverNothingRoutine());

        // No longer hovering over anything.
        Hovered = null;
        _pressed = false;
    }

    private DomeScreen GetScreenUnderIcon()
    {
        var s = Camera.main.WorldToScreenPoint(transform.position);
        var ray = Camera.main.ScreenPointToRay(s);
        var hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            var screen = hit.transform.GetComponent<DomeScreen>();
            if (screen)
                return screen;
        }

        return null;
    }

    private IEnumerator ReleaseOverScreenRoutine()
    {
        var m = Button.GetComponent<MeshRenderer>().material;
        var c = Button.Color;
        var duration = 0.25f;

        m.DOKill();
        transform.DOKill();

        var texts = GetComponentsInChildren<DynamicText>();
        foreach (var text in texts)
            text.pixelSnapTransformPos = false;

        var tween = DOTween.Sequence();
        tween.Append(transform.DOScale(Vector3.zero, duration));
        tween.Join(Button.SetColor(new Color(0, 0, 0, 0)));

        tween.Append(transform.DOScale(Vector3.one, duration));
        tween.Join(Button.SetColor(c));
        tween.Join(transform.DOMove(_homePosition, duration));
        tween.Play();

        yield return new WaitForSeconds(duration * 2);
        foreach (var text in texts)
            text.pixelSnapTransformPos = true;
    }

    private IEnumerator ReleaseOverNothingRoutine()
    {
        var duration = 0.25f;
        transform.DOKill();
        transform.DOMove(_homePosition, duration);
        yield return new WaitForSeconds(duration);
    }

}
