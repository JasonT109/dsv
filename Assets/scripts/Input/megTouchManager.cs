using UnityEngine;
using System.Collections;

public class megTouchManager : Singleton<megTouchManager>
{

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

}
