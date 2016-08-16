using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Networking;
using Meg.UI;
using UnityEngine.Events;

public class debugParameterListUi : MonoBehaviour
{

    // Classes
    // ------------------------------------------------------------

    [Serializable]
    public class SelectedEvent : UnityEvent<string> { }


    // Properties
    // ------------------------------------------------------------

    [Header("Components")]
    public Transform ServerParamEntriesContainer;


    [Header("Prefabs")]

    /** Server param entry UI. */
    public debugServerParamEntryUi ServerParamEntryPrefab;

    /** Event fired when a parameter is selected. */
    public SelectedEvent OnSelected;

    /** The currently selected parameter UI. */
    public debugServerParamEntryUi Selected { get; private set; }



    // Members
    // ------------------------------------------------------------

    /** List of server param UI entries. */
    private readonly List<debugServerParamEntryUi> _serverParams = new List<debugServerParamEntryUi>();


    // Unity Methods
    // ------------------------------------------------------------

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        UpdateServerParamList();
    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateServerParamList()
    {
        var index = 0;
        foreach (var param in serverUtils.InterfaceParameters)
        {
            var entry = GetServerParamEntry(index);
            entry.Text.text = param;
            if (entry.Description)
            {
                var info = serverUtils.GetServerDataInfo(param);
                entry.Description.text = info.description;
            }
            index++;
        }

        for (var i = 0; i < _serverParams.Count; i++)
            _serverParams[i].gameObject.SetActive(i < index);
    }

    private debugServerParamEntryUi GetServerParamEntry(int i)
    {
        if (i >= _serverParams.Count)
        {
            var ui = Instantiate(ServerParamEntryPrefab);
            ui.transform.SetParent(ServerParamEntriesContainer, false);
            ui.Button.onClick.AddListener(() => HandleServerParamSelected(ui));
            _serverParams.Add(ui);
        }

        return _serverParams[i];
    }

    private void HandleServerParamSelected(debugServerParamEntryUi selected)
    {
        if (Selected)
            Selected.On.gameObject.SetActive(false);

        Selected = selected;

        if (Selected)
            Selected.On.gameObject.SetActive(true);

        if (OnSelected != null)
            OnSelected.Invoke(selected.Text.text);
    }
}
