using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** 
 * The object pool maintains lists of available objects separated by type, and provides methods
 * for getting and recycling them. 
 */ 

public class ObjectPool : AutoSingleton<ObjectPool>
{
	// Properties
	// ----------------------------------------------------------------------------

	/** When unparenting put under this transform for a tidy hierarchy (may affect performance). */
	public bool TidyUnparent;

    /** Whether pool is currently being preinstantiated. */
    public bool Preinstantiating { get; private set; }


	// Members
	// ----------------------------------------------------------------------------

	/** The object pool instance. */
	private LocalObjectPool poolInstance;

	/** Get the object pool. */
	private LocalObjectPool pool
	{
		get
		{
			if (poolInstance == null)
				poolInstance = new LocalObjectPool(transform, TidyUnparent);

			return poolInstance;
		}
	}


    // Static Methods
    // ----------------------------------------------------------------------------

    /** Tries to get an object instance from the pool. */
    public static GameObject Get(GameObject template, bool active = true, bool staticBatch = false, string name = null)
        { return Instance.GetObject(template, active, staticBatch, name); }

    /** Tries to get an object instance from the pool, placing it at the given transform's location. */
    public static GameObject GetAt(GameObject template, Transform t, bool reparent = true)
        { return Instance.GetObjectAt(template, t, reparent); }

    /** Tries to get an object instance from the pool, placing it at the given transform's location. */
    public static GameObject GetAt(GameObject template, Vector3 position, Quaternion rotation)
        { return Instance.GetObjectAt(template, position, rotation); }

    /** Get an object with the associated component on it. */
    public static T GetComponent<T>(T component, bool active = true, bool staticBatch = false, string name = null) where T : Component
        { return Instance.GetObjectWithComponent<T>(component, active, staticBatch, name); }

    /** Get an object with the associated component on it, placing it at the given transform's location.. */
    public static T GetComponentAt<T>(T component, Transform t, bool reparent = true) where T : Component
        { return Instance.GetObjectWithComponentAt<T>(component, t, reparent); }

    /** Cleans up an object (whether pooled or not.) */
    public static void Cleanup(GameObject go, bool unparent = false)
    {
        if (!go)
            return;

        if (Instance.IsPooled(go))
            Instance.ReturnObject(go, unparent);
        else
            Destroy(go);
    }


    // Public Interface
    // ----------------------------------------------------------------------------

    /** Preinstantiate a number of objects into the pool. */
    public void Preinstantiate(GameObject template, int n = 1, bool active = true, bool staticBatch = false)
	{
        if (template == null)
            return;

        Preinstantiating = true;
		pool.Preinstantiate(template, n, active, staticBatch);
	    Preinstantiating = false;
	}
	
	/** Get an object of the specified type. */
	protected GameObject GetObject(GameObject template, bool active = true, bool staticBatch = false, string name = null)
	{
        if (template == null)
            return null;

        return pool.GetObject(template, active, staticBatch, name);
	}

    /** Get an object and place it at the given transform's location. */
    protected GameObject GetObjectAt(GameObject template, Transform t, bool reparent = true)
    {
        if (template == null)
            return null;

        var go = pool.GetObject(template, false);
        if (!go)
            return null;
        if (reparent)
            go.transform.parent = t;

        go.transform.position = t.position;
        go.transform.rotation = t.rotation;
        go.SetActive(true);

        return go;
    }

    /** Get an object and place it at the given location. */
    protected GameObject GetObjectAt(GameObject template, Vector3 position, Quaternion rotation)
    {
        if (template == null)
            return null;

        var go = pool.GetObject(template, false);
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.SetActive(true);

        return go;
    }


    /** Get an object with the associated component on it. */
    protected T GetObjectWithComponent<T>(T component, bool active = true, bool staticBatch = false, string name = null) where T : Component
	{
		return pool.GetObjectWithComponent<T>(component, active, staticBatch, name);
	}

    /** Get an object with the associated component on it, parented. */
    protected T GetObjectWithComponentAt<T>(T component, Transform t, bool reparent = true) where T : Component
    {
        if (t == null)
            return null;

        var c = pool.GetObjectWithComponent<T>(component, false);
        var go = c.gameObject;
        if (!go)
            return null;
        if (reparent)
            go.transform.parent = t;

        go.transform.position = t.position;
        go.transform.rotation = t.rotation;
        go.SetActive(true);

        return c;
    }

    /** Return an object to the pool. */
    protected void ReturnObject(GameObject o, bool unparent = false)
	{
		pool.ReturnObject(o, unparent);
	}

    /** Determines if an object is pooled or not. */
    protected bool IsPooled(GameObject go)
    {
        return pool.IsPooled(go);
    }

    /** Log a report of pooled objects. */
    public void Report()
	{
		pool.Report();
	}

}
