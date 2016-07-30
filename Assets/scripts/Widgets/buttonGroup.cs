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

    public bool anyButtonActive()
    {
        // Disable warning 'gameObject.active is obsolete.' (not harmful).
        // The warning is due to buttonControl.active hiding gameObject.active.
        #pragma warning disable 0618

        foreach (var button in buttons)
            if (button.active)
                return true;

        return false;

        // Restore compiler warning for other areas of the code.
        #pragma warning restore 0618
    }
}
