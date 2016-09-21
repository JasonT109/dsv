using System.Linq;
using UnityEngine;
using Meg.Networking;
using UnityEngine.Networking;

public class SetContentOnOtherScreen : MonoBehaviour
{
    /** The target screen and content we wish to set.*/
    public screenData.State Target;


    private void Start()
    {
        // Set up button association automatically.
        var button = GetComponent<buttonControl>();
        if (button)
            button.onPress.AddListener(Apply);
    }

    /** Apply content to matching remote screens. */
    public void Apply()
        { serverUtils.PostScreenStateForType(Target); }
}
