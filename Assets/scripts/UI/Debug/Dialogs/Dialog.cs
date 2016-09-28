using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    /** Dialog's root panel. */
    public CanvasGroup Root;

    /** Backdrop graphic. */
    public Graphic Backdrop;

    /** Title text. */
    public Text Title;

    /** Message text. */
    public Text Message;


    /** Result coming back from this dialog. */
    public enum Result
    {
        Unknown,
        OK,
        Cancel,
        No,
        Yes
    }

    public class ResultEvent : UnityEvent<Result> { }

    /** Result of closing this dialog. */
    public ResultEvent OnClosed = new ResultEvent();

    /** Whether dialog has been closed. */
    private bool _closed;

    /** Initialization. */
    protected virtual void Start()
    {
        // Zoom in the dialog.
        Root.transform.DOScale(Vector3.zero, 0.25f).From();
        Root.DOFade(0, 0.25f).From();
        Backdrop.DOFade(0, 0.25f).From();
    }

    /** Configure the dialog. */
    public virtual void Configure(string title, string message)
    {
        Title.text = title;
        Message.text = message;
    }

    /** Close the dialog. */
    public void Close(Result result = Result.Cancel)
        { StartCoroutine(CloseRoutine(result)); }

    /** Routine to close the dialog. */
    private IEnumerator CloseRoutine(Result result)
    {
        // Guard against multiple close attempts.
        if (_closed)
            yield break;

        _closed = true;

        // Notify listeners of the result.
        OnClosed.Invoke(result);

        // Zoom out the dialog.
        Root.interactable = false;
        Root.transform.DOScale(Vector3.zero, 0.25f);
        Root.DOFade(0, 0.25f);
        Backdrop.DOFade(0, 0.25f);

        // Kill dialog after a short delay.
        yield return new WaitForSeconds(0.25f);
        gameObject.Cleanup();
    }
}
