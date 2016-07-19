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


    // Protected Static Methods
    // -----------------------------------------------------

    /** Manually set up the singleton instance. */
    protected static void SetInstance(T value)
    {
        _instance = value;
    }


    // Static Members
    // -----------------------------------------------------

    /** The singleton instance. */
    protected static T _instance;


}
