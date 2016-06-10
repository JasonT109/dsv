using UnityEngine;
using System.Collections;

public class buttonGroup : MonoBehaviour {

    public GameObject[] buttons;

    public void toggleButtons(GameObject b)
    {
        foreach (var button in buttons)
        {
            var bScript = button.GetComponent<buttonControl>();
            bScript.toggleButton(b);
            Resources.UnloadUnusedAssets();
        }
    }
}
