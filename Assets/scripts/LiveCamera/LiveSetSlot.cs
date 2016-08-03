using UnityEngine;
using System.Collections;

public class LiveSetSlot : MonoBehaviour {

    public GameObject cameraManager;
    public GameObject buttonGrp;

    public buttonGroup b;

    void Awake()
    {
        if (!cameraManager)
            cameraManager = ObjectFinder.LocateGameObject<CustomLiveFeedManager>();
    }

	// Use this for initialization
	void Start ()
    {
        buttonGroup b = buttonGrp.GetComponent<buttonGroup>();
        for (int i = 0; i < b.buttons.Length; i++)
        {
            if (b.buttons[i].GetComponent<buttonControl>().active)
            {
                int slot = int.Parse(b.buttons[i].GetComponent<buttonControl>().serverName);
                cameraManager.GetComponent<CustomLiveFeedManager>().SetSlot(slot);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (b.changed)
        {
            b.changed = false;
            for (int i = 0; i < b.buttons.Length; i++)
            {
                if (b.buttons[i].GetComponent<buttonControl>().active)
                {
                    int slot = int.Parse(b.buttons[i].GetComponent<buttonControl>().serverName);
                    cameraManager.GetComponent<CustomLiveFeedManager>().SetSlot(slot);
                }
            }
        }
	}
}
