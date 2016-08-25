using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class DialogYesNo : MonoBehaviour
{
    /** Dialog's root panel. */
    public CanvasGroup Root;

    /** Backdrop graphic. */
    public Graphic Backdrop;

    /** Title text. */
    public Text Title;

    /** Message text. */
    public Text Message;

    /** Yes button. */
    public Button YesButton;

    /** No button. */
    public Button NoButton;


    /** Result coming back from this dialog. */
    public enum DialogResult
    {
        Unknown,
        No,
        Yes
    }

    public class ResultEvent : UnityEvent<DialogResult> { }

    /** Result of closing this dialog. */
    public ResultEvent OnClosed = new ResultEvent();

    /** Whether dialog has been closed. */
    private bool _closed;

    /** Initialization. */
    private void Start()
    {
        // Zoom in the dialog.
        Root.transform.DOScale(Vector3.zero, 0.25f).From();
        Root.DOFade(0, 0.25f).From();
        Backdrop.DOFade(0, 0.25f).From();
    }

    /** Configure the dialog. */
    public void Configure(string title, string message)
    {
        Title.text = title;
        Message.text = message;
    }

    /** Disabling. */
    private void OnDisable()
    {
    }

    /** Updating. */
    private void Update()
    {
        // Cancel the dialog if needed.
        if (Input.GetKeyDown(KeyCode.Escape))
            No();
    }

    /** Handle the 'yes' button being pressed. */
    public void Yes()
        { StartCoroutine(CloseRoutine(DialogResult.Yes)); }

    /** Handle the 'no' button being pressed. */

    public void No()
        { StartCoroutine(CloseRoutine(DialogResult.No));}

    /** Routine to close the dialog. */
    private IEnumerator CloseRoutine(DialogResult result)
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
        Destroy(gameObject);
    }
}
