using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class DialogYesNo : Dialog
{

    /** Yes button. */
    public Button YesButton;

    /** No button. */
    public Button NoButton;

    /** Updating. */
    protected virtual void Update()
    {
        // Cancel the dialog if needed.
        if (Input.GetKeyDown(KeyCode.Escape))
            No();
    }

    /** Handle the 'yes' button being pressed. */
    public void Yes()
        { Close(Result.Yes); }

    /** Handle the 'no' button being pressed. */
    public void No()
        { Close(Result.No);}

}
