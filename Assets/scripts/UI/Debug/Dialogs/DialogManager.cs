using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class DialogManager : Singleton<DialogManager>
{

    /** Prefab for a yes/no dialog. */
    public DialogYesNo DialogYesNoPrefab;

    /** Current dialog instance. */
    public DialogYesNo Current { get; private set; }


    /** Whether a dialog is already showing. */
    public bool Showing
        { get { return Current != null; } }

    /** Display a yes/no dialog. */
    public DialogYesNo ShowYesNo(string title, string message, UnityAction<DialogYesNo.DialogResult> action = null)
    {
        if (Showing)
            return null;

        Current = Instantiate(DialogYesNoPrefab);
        Current.transform.SetParent(transform, false);
        Current.Configure(title.ToUpper(), message);

        if (action != null)
            Current.OnClosed.AddListener(action);

        return Current;
    }
    
}
