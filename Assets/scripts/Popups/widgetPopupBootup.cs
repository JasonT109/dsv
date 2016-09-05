using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.EventSystem;
using Meg.Networking;
using UnityEngine.UI;

public class widgetPopupBootup : widgetPopup
{

    [Header("Components")]

    public CanvasGroup Backdrop;
    public CanvasGroup Logo;
    public CanvasGroup Flash;
    public CanvasGroup Burst;
    public CanvasGroup Black;
    public CanvasGroup Code;
    public CanvasGroup Online;

    /** Show this popup. */
    public override void Show(popupData.Popup popup)
    {
        if (string.IsNullOrEmpty(popup.Title))
            popup.Title = serverUtils.VesselData.PlayerVesselName.ToUpper() + " ONLINE";

        if (Equals(popup.Message, megEventPopup.DefaultMessage))
            popup.Message = "";

        base.Show(popup);
    }

    /** Animate the popup into place. */
    protected override void AnimateIn()
    {
        Backdrop.gameObject.SetActive(true);
        Logo.gameObject.SetActive(true);
        Logo.alpha = 0;
        Logo.DOFade(1, 2).SetDelay(1);

        Flash.gameObject.SetActive(false);
        Burst.gameObject.SetActive(false);
        Black.gameObject.SetActive(false);
        Code.gameObject.SetActive(false);
        Online.gameObject.SetActive(false);

        StartCoroutine(BootupRoutine());
    }

    /** Bootup animation sequence. */
    private IEnumerator BootupRoutine()
    {
        // Wait a bit on the boot screen
        yield return new WaitForSeconds(1f);

        // Kick off screen flash and burst animations.
        Flash.gameObject.SetActive(true);
        Flash.DOFade(0, 0.5f);
        Burst.gameObject.SetActive(true);
        Burst.transform.DOScale(Vector3.zero, 1f).From();
        Burst.DOFade(0, 1f);

        // Wait for initial logo fade to complete.
        yield return new WaitForSeconds(2.25f);
        Flash.gameObject.SetActive(false);
        Burst.gameObject.SetActive(false);

        // Jump to a black screen briefly.
        Logo.gameObject.SetActive(false);
        Black.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        Black.gameObject.SetActive(false);

        // Start the code display sequence.
        var codeDuration = Mathf.Max(0, serverUtils.GetServerData("bootCodeDuration", 3f));
        Code.gameObject.SetActive(true);
        if (codeDuration > 0)
            yield return new WaitForSeconds(codeDuration);
        Code.gameObject.SetActive(false);

        // Show online graphic.
        Backdrop.gameObject.SetActive(false);
        Online.gameObject.SetActive(true);
        Online.alpha = 1;

        // Set up each tick as hidden, initially.
        var tickContainer = Online.transform.FindChild("Title/Progress");
        var ticks = tickContainer ? tickContainer.GetComponentsInChildren<CanvasGroup>() : new CanvasGroup[] {};
        foreach (var t in ticks)
            t.alpha = 0;

        // Reveal ticks one by one.
        var step = (100f / ticks.Length);
        var next = step;
        foreach (var tick in ticks)
        {
            while (serverUtils.GetServerData("bootProgress") < next)
                yield return 0;

            tick.alpha = 1;
            next = Mathf.Clamp(next + step, 0, 100f);
        }

        // Wait for fade to finish.
        yield return new WaitForSeconds(0.5f);
        Online.DOFade(0, 1);
        yield return new WaitForSeconds(1.25f);

        // Hide everything.
        Backdrop.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Black.gameObject.SetActive(false);
        Code.gameObject.SetActive(false);
        Online.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
