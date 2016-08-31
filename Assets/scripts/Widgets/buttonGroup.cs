using UnityEngine;
using System.Collections;
using System.Linq;

public class buttonGroup : MonoBehaviour {

    public GameObject[] buttons;
    public bool changed = false;
    public GameObject lastButton { get; private set; }

    public void Awake()
    {
        foreach (var button in buttons)
        {
            if (!button)
                continue;

            var bScript = button.GetComponent<buttonControl>();
            if (bScript && bScript.active)
                lastButton = button;
        }
    }

    public void toggleButtons(GameObject b, bool forceOn = false)
    {
        changed = true;

        foreach (var button in buttons)
        {
            var bScript = button.GetComponent<buttonControl>();
            bScript.toggleButton(b, forceOn);
            //Resources.UnloadUnusedAssets();

            if (bScript.active)
                lastButton = button;
        }
    }

    public void toggleAllOff()
    {
        foreach (GameObject b in buttons)
        {
            if (b.GetComponent<buttonControl>().active && b.GetComponent<buttonControl>().canToggleOff)
                toggleButtons(b);
        }
    }

    public void toggleButtonOn(GameObject b)
        { toggleButtons(b, true); }

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

    public void Add(GameObject b)
    {
        var list = buttons.ToList();
        list.Add(b);

        buttons = list.ToArray();
    }
}
