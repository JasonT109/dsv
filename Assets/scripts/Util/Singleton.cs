using UnityEngine;
using System.Collections;

/** 
 * Convenience class for defining singleton components. 
 */ 

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	// Public Static Methods
	// -----------------------------------------------------

	/** Returns the instance of this singleton. */
	public static T Instance
	{
		get
		{
            if (!_instance)
                EnsureInstanceExists();

			return _instance;
		}
	}

    /** Determines if this singleton has an active instance. */
    public static bool HasInstance
        { get { return _instance != null; } }

    /** Ensures the singleton instance exists. */
    public static void EnsureInstanceExists()
    {
        if (!_instance)
            _instance = (T) FindObjectOfType(typeof(T));
    }


    // Static Members
    // -----------------------------------------------------

    /** The singleton instance. */
    private static T _instance;

}
