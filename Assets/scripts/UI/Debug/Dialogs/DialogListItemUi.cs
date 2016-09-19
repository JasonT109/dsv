using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class DialogListItemUi : MonoBehaviour
{
    /** Item's button. */
    public Button Button;

    /** Item's label. */
    public Text Label;

    /** Item's on graphic. */
    public Graphic On;

    /** Item data. */
    private DialogList.Item _item;
    public DialogList.Item Item
    {
        get { return _item; }
        set { SetItem(value); }
    }

    private void SetItem(DialogList.Item value)
    {
        _item = value;
        Label.text = _item.Name;
    }

    /** Item selected state. */
    private bool _selected;
    public bool Selected
    {
        get { return _selected; }
        set { SetSelected(value); }
    }

    private void SetSelected(bool value)
    {
        _selected = value;
        On.gameObject.SetActive(value);
    }

}
