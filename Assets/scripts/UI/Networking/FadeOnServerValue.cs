using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;

public class FadeOnServerValue : MonoBehaviour
{

    /** Server data value to monitor. */
    public string linkDataString;

    /** Threshold for enabling/disabling. */
    public float threshold = 1;

    /** Whether objects are enabled when value goes above threshold. */
    public bool thresholdGreaterThan = false;

    /** Whether objects are enabled if value equals threshold. */
    public bool thresholdEqualTo = true;

    public CanvasGroup Group;
    public float FadeDuration = 0.5f;

    private bool _active;


    void Start()
    {
        if (!Group)
            Group = GetComponent<CanvasGroup>();

        _active = Group.alpha > 0;

        UpdateFade();
    }
	
	private void Update()
        { UpdateFade(); }

    private void UpdateFade()
    {
        if (!serverUtils.IsReady())
            return;

        var value = serverUtils.GetServerData(linkDataString);
        var active = thresholdGreaterThan ? value > threshold : value <= threshold;
        if (thresholdEqualTo)
            active = Mathf.Approximately(value, threshold);

        if (_active == active)
            return;

        Group.DOKill();
        Group.DOFade(active ? 1 : 0, FadeDuration);

        _active = active;
    }
}
