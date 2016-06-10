using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValueFromServer : NetworkBehaviour
{
    public string linkDataString = "depth";

    void Update()
    {
        gameObject.GetComponent<TextMesh>().text = serverUtils.GetServerDataAsText(linkDataString);
    }
}
