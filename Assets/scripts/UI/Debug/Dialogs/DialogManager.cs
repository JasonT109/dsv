using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class DialogManager : Singleton<DialogManager>
{

    /** Prefab for a yes/no dialog. */
    public DialogYesNo DialogYesNoPrefab;

    /** Prefab for a list dialog. */
    public DialogList DialogListPrefab;

    /** Current dialog instance. */
    public Dialog Current { get; private set; }

    /** Whether a dialog is already showing. */
    public bool Showing
        { get { return Current != null; } }


    /** Display a yes/no dialog. */
    public DialogYesNo ShowYesNo(string title, string message, UnityAction<Dialog.Result> action = null)
    {
        if (Showing)
            return null;

        var dialog = Instantiate(DialogYesNoPrefab);
        dialog.transform.SetParent(transform, false);
        dialog.Configure(title.ToUpper(), message);
        Current = dialog;

        if (action != null)
            Current.OnClosed.AddListener(action);

        return dialog;
    }

    /** Display a list dialog. */
    public DialogList ShowList(string title, string message, IEnumerable<DialogList.Item> items)
        { return ShowList(title, message, items, null, null); }

    /** Display a list dialog. */
    public DialogList ShowList(string title, string message, IEnumerable<DialogList.Item> items, UnityAction<DialogList.Item> chosen)
        { return ShowList(title, message, items, null, chosen); }

    /** Display a list dialog. */
    public DialogList ShowList(string title, string message, IEnumerable<DialogList.Item> items, string selectedId, UnityAction<DialogList.Item> chosen = null)
    {
        if (Showing)
            return null;

        var dialog = Instantiate(DialogListPrefab);
        dialog.transform.SetParent(transform, false);
        dialog.Configure(title.ToUpper(), message, items, selectedId);
        Current = dialog;

        if (chosen != null)
            dialog.OnChosen.AddListener(chosen);

        return dialog;
    }



}
