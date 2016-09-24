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
        {
            return Id.GetHashCode();
        }
    }

    public bool Multiselect { get; private set; }
    public Item Selected { get; private set; }

    public class ItemChosenEvent : UnityEvent<Item> { }
    public ItemChosenEvent OnChosen = new ItemChosenEvent();

    public class ItemsChosenEvent : UnityEvent<List<Item>> { }
    public ItemsChosenEvent OnItemsChosen = new ItemsChosenEvent();

    private readonly List<DialogListItemUi> _uis = new List<DialogListItemUi>();

    /** Configure the dialog. */
    public virtual void Configure(string title, string message, IEnumerable<Item> items)
    {
        Title.text = title;
        Message.text = message;
        PopulateItems(items);
    }

    /** Configure the dialog flor single selection. */
    public virtual void Configure(string title, string message, IEnumerable<Item> items, string selectedId)
    {
        Title.text = title;
        Message.text = message;
        PopulateItems(items);

        if (!string.IsNullOrEmpty(selectedId))
            Select(selectedId, true);
        else if (_uis.Count > 0)
            Select(_uis[0].Item.Id, true);
    }

    /** Configure the dialog for multiple selection. */
    public virtual void Configure(string title, string message, IEnumerable<Item> items, IEnumerable<string> selectedIds)
    {
        Multiselect = true;
        Title.text = title;
        Message.text = message;
        PopulateItems(items);
        Select(selectedIds, true);
    }

    /** Popuplate the list with item UI elements. */
    protected virtual void PopulateItems(IEnumerable<Item> items)
    {
        _uis.Clear();
        foreach (var item in items)
        {
            var ui = Instantiate(ItemUiPrefab);
            ui.transform.SetParent(ItemContainer, false);
            ui.Item = item;
            ui.Button.onClick.AddListener(() => OnItemClicked(ui));
            _uis.Add(ui);
        }
    }

    /** Updating. */
    protected virtual void Update()
    {
        // Cancel the dialog if needed.
        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    /** Handle the 'OK' button being pressed. */
    public void OK()
    {
        if (OnChosen != null)
            OnChosen.Invoke(Selected);

        if (OnItemsChosen != null)
            OnItemsChosen.Invoke(GetSelectedItems());

        Close(Result.OK);
    }

    /** Handle the 'cancel' button being pressed. */
    public void Cancel()
    {
        Close();
    }

    private IEnumerator CenterOnItem(DialogListItemUi ui)
    {
        yield return new WaitForSeconds(0.1f);
        ScrollRect.CenterOnItem(ui.GetComponent<RectTransform>());
    }

    private void OnItemClicked(DialogListItemUi ui)
    {
        if (Multiselect)
            Toggle(ui.Item.Id);
        else
            Select(ui.Item.Id);
    }

    private void Select(string id, bool centerOnSelected = false)
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

    private void Toggle(string id)
    {
        foreach (var ui in _uis)
            if (ui.Item.Id == id)
                ui.Selected = !ui.Selected;
    }

    private void Select(IEnumerable<string> ids, bool centerOnSelected = false)
    {
        var idList = new List<string>(ids);

        Selected = default(Item);
        var hasSelected = false;

        foreach (var ui in _uis)
        {
            ui.Selected = idList.Contains(ui.Item.Id);
            if (hasSelected)
                continue;

            hasSelected = true;
            Selected = ui.Item;
            if (centerOnSelected && ScrollRect)
                StartCoroutine(CenterOnItem(ui));
        }
    }

    private List<Item> GetSelectedItems()
    {
        var items = new List<Item>();
        foreach (var ui in _uis)
        {
            if (ui.Selected)
                items.Add(ui.Item);
        }

        return items;
    }

}
