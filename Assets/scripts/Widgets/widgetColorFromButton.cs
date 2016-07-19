using UnityEngine;
using System.Collections;

public class widgetColorFromButton : MonoBehaviour
{
    public GameObject button;
    buttonControl bc;

	void Start ()
    {
        bc = button.GetComponent<buttonControl>();
	}
	
    void OnEnable()
    {
        if (bc != null)
        {
            Color c = bc.GetThemeColor(3);
            gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", c);
        }
        else
        {
            bc = button.GetComponent<buttonControl>();
            Color c = bc.GetThemeColor(3);
            gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", c);
        }
    }
}
