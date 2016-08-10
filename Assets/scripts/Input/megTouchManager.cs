using UnityEngine;
using System.Collections;

public class megTouchManager : Singleton<megTouchManager>
{

/** 
 * Keep touch manager resident between screens in Standalone builds,
 * to prevent issues with the TouchScript plugin losing input focus.
 * No need to apply this logic in the Unity Editor, however.
 */

#if !UNITY_EDITOR

    private void Awake()
	{
        // If there's already an instance in the scene, destroy this one.
	    if (HasInstance)
	        Destroy(gameObject);
	    else
	    {
	        // Otherwise, make this instance persistent.
	        DontDestroyOnLoad(gameObject);
            SetInstance(this);
	    }
	}
#endif

}
