using UnityEngine;
using System.Collections;

/**
 * A singleton that automatically creates its own instance.
 */

public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;

    protected static bool _quitting;
	
	/** Returns the instance of this singleton. */
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T) FindObjectOfType(typeof(T));
				if (instance == null)
					CreateInstance();
			}
			
			return instance;
		}
	}

    /** Returns whether there is an existing instance of this singleton. */
    public static bool HasExistingInstance
        { get { return instance != null; } }

	/** Create the singleton instance. */
	protected static void CreateInstance()
	{
	    if (_quitting)
	    {
	        Debug.LogError("AutoSingleton.CreateInstance(): Attempt to instantiate a singleton while quitting.");
	        return;
	    }

	    var name = typeof(T).Name;
		var go = new GameObject(name);
		go.AddComponent<T>();
		instance = go.GetComponent<T>();
	}

    /** Detect when Unity is shutting down. */
    protected virtual void OnApplicationQuit()
    {
        _quitting = true;
    }

}
