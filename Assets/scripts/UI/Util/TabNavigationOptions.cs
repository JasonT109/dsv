using UnityEngine;
using System.Collections;

public class TabNavigationOptions : MonoBehaviour
{

    /** Possible tab navigation behaviours. */
    public enum TabBehaviour
    {
        Normal = 0,
        IgnoreTab,
    }

    /** The behaviour of this object with respect to tab navigation. */
    public TabBehaviour Behaviour = TabBehaviour.Normal;

}
