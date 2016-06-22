using UnityEngine;
using UnityEngine.Networking;
using Meg.Networking;

public class textValueFromServer : NetworkBehaviour
{
    public string linkDataString = "depth";
    public float updateTick = 0.2f;
    private float nextUpdate = 0;

    void Start()
    {
        nextUpdate = Time.time;
    }

    void Update()
    {
        if (Time.time > nextUpdate)
        {
            nextUpdate = Time.time + updateTick;
            gameObject.GetComponent<TextMesh>().text = serverUtils.GetServerDataAsText(linkDataString);
        }
    }
}
