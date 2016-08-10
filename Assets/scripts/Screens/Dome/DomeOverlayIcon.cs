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

    private GameObject _underlayIcon;

    private void Awake()
    {
        _homePosition = transform.position;

        if (!Button)
            Button = GetComponent<buttonControl>();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += OnTapped;
        GetComponent<PressGesture>().Pressed += OnPressed;
        GetComponent<ReleaseGesture>().Released += OnReleased;

        // Create an underlay icon (unless we're an underlay ourselves!)
        if (!_underlayIcon)
            StartCoroutine(CreateUnderlayIcon());
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

    private IEnumerator CreateUnderlayIcon()
    {
        // Check that this object is not itself an underlay.
        // This is done to prevent infinite cloning recursion.
        yield return new WaitForSeconds(0.1f);
        if (gameObject.name.Contains("Underlay"))
            yield break;

        // Create a clone of this icon, and make it a sibling.
        var go = Instantiate(gameObject);
        go.name = gameObject.name + "Underlay";

        // Push underlay back into the screen.
        var p = transform.position;
        go.transform.SetParent(transform.parent, false);
        go.transform.localScale = transform.localScale;
        go.transform.position = new Vector3(p.x, p.y, p.z + 1);

        // Disable interactive elements.
        go.GetComponent<buttonControl>().enabled = false;
        foreach (var gesture in go.GetComponentsInChildren<Gesture>())
            gesture.enabled = false;

        _underlayIcon = go;
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
        var invisible = new Color(0, 0, 0, 0);
        var duration = 0.25f;

        m.DOKill();
        transform.DOKill();

        var tween = DOTween.Sequence();
        tween.Append(transform.DOScale(Vector3.zero, duration)
            .OnComplete(() => transform.position = _homePosition));

        tween.Join(Button.SetColor(invisible));

        var texts = GetComponentsInChildren<DynamicText>();
        for (var i = 0; i < texts.Length; i++)
        {
            var text = texts[i];
            var textOriginalColor = text.color;
            text.pixelSnapTransformPos = false;
            var textTween = DOTween.Sequence();
            textTween.Append(DOTween.To(() => text.color, x => text.color = x, invisible, duration));
            textTween.Append(DOTween.To(() => text.color, x => text.color = x, textOriginalColor, duration));
            tween.Join(textTween);
        }

        tween.Append(transform.DOScale(Vector3.one, duration));
        tween.Join(Button.SetColor(c));
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
