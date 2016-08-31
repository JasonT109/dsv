using System;
using UnityEngine;
using System.Collections;
using Meg.Networking;

public class DCCServerValueSwitcharoo : MonoBehaviour
{
    [Serializable]
    public struct Option
    {
        public int value;
        public GameObject gameObject;
    }

    public string ServerParameter;
    public Option[] Options;

    void OnEnable()
        { UpdateOptions(); }

    void Update()
        { UpdateOptions(); }

    void UpdateOptions()
    {
        var value = serverUtils.GetServerData(ServerParameter, 0);
        foreach (var option in Options)
            option.gameObject.SetActive(Mathf.Approximately(option.value, value));
	}
}
