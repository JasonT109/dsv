using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    /** Structure for items. */
    public struct Item
    {
        public string Name;
        public string Value;

        public override bool Equals(object o)
        {
            if (!(o is Item))
                return false;

            var s = (Item) o;
            return Value == s.Value;
        }

        public override int GetHashCode()
            { return Value.GetHashCode(); }
    }

    public Item Selected { get; private set; }
    
    public class ItemChosenEvent : UnityEvent<Item> { }
    public ItemChosenEvent OnChosen = new ItemChosenEvent();

    private readonly List<DialogListItemUi> _uis = new List<DialogListItemUi>();

    /** Configure the dialog. */
    public virtual void Configure(string title, string message, List<Item> items)
        { Configure(title, message, items, default(Item)); }

    /** Configure the dialog. */
    public virtual void Configure(string title, string message, List<Item> items, Item selected)
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

        if (!string.IsNullOrEmpty(selected.Value))
            Select(selected);
        else if (items.Count > 0)
            Select(items[0]);
    }


    /** Updating. */
    protected virtual void Update()
    {
        // Cancel the dialog if needed.
        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    /** Select the given item. */
    public void Select(Item item)
    {
        foreach (var ui in _uis)
            ui.Selected = Equals(ui.Item, item);

        Selected = item;
    }

    /** Handle the 'OK' button being pressed. */
    public void OK()
        { Close(Result.OK); }

    /** Handle the 'cancel' button being pressed. */
    public void Cancel()
        { Close();}

}
