using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.EventSystem;
using Meg.Networking;
using UnityEngine.UI;

public class widgetPopupBootup_RSide : widgetPopup
{

    [Header("Components")]

    public CanvasGroup Backdrop;
    public CanvasGroup Logo;
    public CanvasGroup Flash;
    public CanvasGroup Burst;
    public CanvasGroup Black;
    public CanvasGroup Code;
    public CanvasGroup Online;
    public CanvasGroup FinalPop;

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
        FinalPop.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(1f);
        Code.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        Code.gameObject.SetActive(false);

        Logo.gameObject.SetActive(true);
        Backdrop.gameObject.SetActive(true);
        yield return new WaitForSeconds(7f);
        Logo.gameObject.SetActive(false);
        Backdrop.gameObject.SetActive(false);


        // Show online graphic.
        Backdrop.gameObject.SetActive(false);
        Online.gameObject.SetActive(false);
        Online.DOFade(0, 1).SetDelay(1.5f);

        // Progress animation.
        var ticks = GetComponentsInChildren<CanvasGroup>();
        foreach (var tick in ticks)
            tick.alpha = 0;
        StartCoroutine(ProgressRoutine(ticks, 1f));

        // Wait for progress bar to finish.
        yield return new WaitForSeconds(2.5f);

        // Hide everything.
        Backdrop.gameObject.SetActive(false);
        Logo.gameObject.SetActive(false);
        Black.gameObject.SetActive(false);
        Code.gameObject.SetActive(false);
        Online.gameObject.SetActive(false);
        FinalPop.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private IEnumerator ProgressRoutine(CanvasGroup[] ticks, float duration)
    {
        var maxDelay = (duration / ticks.Length) * 3;
        foreach (var tick in ticks)
        {
            tick.alpha = 1;
            var delay = Random.Range(0, Mathf.Min(maxDelay, duration));
            duration = Mathf.Max(0, duration - delay);
            yield return new WaitForSeconds(delay);
        }
    }
}
