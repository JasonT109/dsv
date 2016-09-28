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
	    var active = Button && Button.active;

        foreach (var go in Objects)
            if (go && go.activeSelf != active)
                go.SetActive(active);

        foreach (var b in Behaviours)
            if (b)
                b.enabled = active;
    }
}
