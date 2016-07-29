using UnityEngine;
using System.Collections;

/**
 * A singleton that automatically creates its own instance.
 */

public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;
	
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
    {
        get { return instance != null; }
    }

	/** Create the singleton instance. */
	protected static void CreateInstance()
	{
		string name = typeof(T).Name;
		GameObject go = new GameObject(name);
		go.AddComponent<T>();
		instance = go.GetComponent<T>();
	}

}
