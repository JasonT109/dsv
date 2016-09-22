using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Meg.UI;
using UnityEngine.Events;

public class DialogList : Dialog
{

    /** OK button. */
    public Button OkButton;

    /** Cancel button. */
    public Button CancelButton;

    /** Item container. */
    public Transform ItemContainer;

    /** Prefab for list items. */
    public DialogListItemUi ItemUiPrefab;

    /** Scrolling view. */
    public megScrollRect ScrollRect;

    /** Structure for items. */
    public struct Item
    {
        public string Name;
        public string Id;

        public override bool Equals(object o)
        {
            if (!(o is Item))
                return false;

            var s = (Item) o;
            return Id == s.Id;
        }

        public override int GetHashCode()
            { return Id.GetHashCode(); }
    }

    public Item Selected { get; private set; }
    
    public class ItemChosenEvent : UnityEvent<Item> { }
    public ItemChosenEvent OnChosen = new ItemChosenEvent();

    private readonly List<DialogListItemUi> _uis = new List<DialogListItemUi>();

    /** Configure the dialog. */
    public virtual void Configure(string title, string message, IEnumerable<Item> items)
        { Configure(title, message, items, null); }

    /** Configure the dialog. */
    public virtual void Configure(string title, string message, IEnumerable<Item> items, string selectedId)
    {
        Title.text = title;
        Message.text = message;

        _uis.Clear();

        foreach (var item in items)
        {
            var ui = Instantiate(ItemUiPrefab);
            ui.transform.SetParent(ItemContainer, false);
            ui.Item = item;
            ui.Button.onClick.AddListener(() => Select(ui.Item));
            _uis.Add(ui);
        }

        if (!string.IsNullOrEmpty(selectedId))
            Select(selectedId, true);
        else if (_uis.Count > 0)
            Select(_uis[0].Item.Id, true);
    }


    /** Updating. */
    protected virtual void Update()
    {
        // Cancel the dialog if needed.
        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    /** Select the given item. */
    public void Select(string id, bool centerOnSelected = false)
    {
        Selected = default(Item);

        foreach (var ui in _uis)
        {
            ui.Selected = Equals(ui.Item.Id, id);
            if (ui.Selected)
                Selected = ui.Item;

            if (centerOnSelected && ScrollRect && ui.Selected)
                StartCoroutine(CenterOnItem(ui));
        }
    }

    /** Select the given item. */
    public void Select(Item item, bool centerOnSelected = false)
    {
        Selected = default(Item);
        foreach (var ui in _uis)
        {
            ui.Selected = Equals(ui.Item, item);
            if (ui.Selected)
                Selected = ui.Item;

            if (centerOnSelected && ScrollRect && ui.Selected)
                StartCoroutine(CenterOnItem(ui));
        }
    }

    /** Handle the 'OK' button being pressed. */
    public void OK()
    {
        if (OnChosen != null)
            OnChosen.Invoke(Selected);

        Close(Result.OK);
    }

    /** Handle the 'cancel' button being pressed. */
    public void Cancel()
        { Close();}


    private IEnumerator CenterOnItem(DialogListItemUi ui)
    {
        yield return new WaitForSeconds(0.1f);
        ScrollRect.CenterOnItem(ui.GetComponent<RectTransform>());
    }
}
