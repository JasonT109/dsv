using UnityEngine;
using System.Collections;
using Meg.Networking;

public class DCCMissionWindowSwitcharoo : MonoBehaviour
{
    public struct WindowForValue
    {
        public int value;
        public GameObject window;
    }

    public string ServerParameter;
    public WindowForValue[] Windows;

    void Update()
    {
        var value = serverUtils.GetServerData(ServerParameter);
        foreach (var w in Windows)
            w.window.SetActive(Mathf.Approximately(w.value, value));
	}
}
