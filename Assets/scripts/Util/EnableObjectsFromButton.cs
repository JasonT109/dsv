using UnityEngine;
using System.Collections;

public class EnableObjectsFromButton : MonoBehaviour
{

    public buttonControl Button;
    public GameObject[] Objects;
    public MonoBehaviour[] Behaviours;

    void Start()
	{
	    if (!Button)
	        Button = GetComponent<buttonControl>();
	}
	
	void Update()
    {
        foreach (var go in Objects)
            if (go)
                go.SetActive(Button && Button.active);

        foreach (var b in Behaviours)
            if (b)
                b.enabled = Button && Button.active;
    }
}
