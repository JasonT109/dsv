using UnityEngine;
using System.Collections;

public class buttonGroup : MonoBehaviour {

    public GameObject[] buttons;
    public bool changed = false;

    public void toggleButtons(GameObject b)
    {
        changed = true;

        foreach (var button in buttons)
        {
            var bScript = button.GetComponent<buttonControl>();
            bScript.toggleButton(b);
            Resources.UnloadUnusedAssets();
        }
    }
}
