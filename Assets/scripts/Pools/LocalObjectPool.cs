using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** 
 * The object pool maintains lists of available objects separated by type, 
 * and provides methods for getting and recycling them. 
 */ 

public class LocalObjectPool
{
	// Members
	// ----------------------------------------------------------------------------
	
	/** Lists of objects mapped to object types. */
	private Dictionary<string, Stack<GameObject>> objectLists;
	
	/** Optionally count object instances to generate preinstantiation data. */
	private Dictionary<GameObject, int> instanceCounts;

    /** Lookup for determining which objects were created via object pool. */
    private Dictionary<GameObject, bool> pooledLookup;

	/** A parent transform used as a container for instantiated objects. */
	private Transform transform;

	/** When unparenting put under this transform for a tidy hierarchy (may effect performance). */
	private bool tidyUnparent;


	// Life Cycle
	// ----------------------------------------------------------------------------
	
	/** Constructor. */
	public LocalObjectPool(Transform transform, bool tidyUnparent = true)
	{
		// Create the object list and instance count maps.
		objectLists = new Dictionary<string, Stack<GameObject>>();
		instanceCounts = new Dictionary<GameObject, int>();
        pooledLookup = new Dictionary<GameObject, bool>();


        this.transform = transform;
		this.tidyUnparent = tidyUnparent;
	}
	
	
	// Public Interface
	// ----------------------------------------------------------------------------
	
	/** Preinstantiate a number of objects into the pool. */
	public void Preinstantiate(GameObject template, int n = 1, bool active = true, bool staticBatch = false)
	{
		// Check that the template exists.
		if (template == null)
			return;
		
		// Get the list of objects for the object type.
		Stack<GameObject> objects = GetObjectList(template.name);
		
		// Create n objects and add them to the pool.
		for (int i = 0; i < n; i++)
		{
			// Create a new object instance.
			GameObject o = CreateObject(template, active, staticBatch);
			
			// Parent the preinstantiated objects to the
			// pool if in editor mode to keep the scene tidy.
			#if UNITY_EDITOR
			o.transform.parent = transform;
			#endif
			
			// Add object in to the list.
			objects.Push(o);
		}
	}

    /** Determines if an object is pooled or not. */
    public bool IsPooled(GameObject go)
    {
        return pooledLookup.ContainsKey(go);
    }

    /** Get an instantiated object of the specified template. */
    public GameObject GetObject(GameObject template, bool active = true, bool staticBatch = false, string name = null)
	{
		// Use the type name if not specified.
		if (name == null)
			name = template.name;

		// Get the list of available objects.
		Stack<GameObject> objects = GetObjectList(name);
		
		// Instantiate a new object if there are none in the list.
		if (objects.Count <= 0)
		{
			// Create a new object instance.
			GameObject o = CreateObject(template, active, staticBatch, name);

            // Track instance counts.
            #if UNITY_EDITOR
            if (instanceCounts.ContainsKey(template))
                instanceCounts[template]++;
            else
                instanceCounts.Add(template, 1);
            #endif

            return o;
		}
		
		// Otherwise, remove and return the last object.
		else
		{
			GameObject o = objects.Pop();
			
			if (active)
				o.SetActive(true);
			
			return o;
		}
	}

	/** Get an object with the associated component on it. */
	public T GetObjectWithComponent<T>(T component, bool active = true, bool staticBatch = false, string name = null) where T : Component
	{
		return GetObject(component.gameObject, active, staticBatch, name).GetComponent<T>();
	}

	/** Return a object to the pool. */
	public void ReturnObject(GameObject o, bool unparent = false)
	{
		// Check that the object exists.
		if (o == null)
			return;
		
		// Deactivate the object.
		o.SetActive(false);

		// Unparent.
		if (unparent)
		{
			if (tidyUnparent)
				o.transform.SetParent(transform);	
			else
				o.transform.SetParent(null);
		}
		
		// Get the list of objects for the object type.
		Stack<GameObject> objects = GetObjectList(o.name);
		
		// Add the object back in to the list.
		objects.Push(o);
	}
	
	/** Log a report of pooled objects. */
	public void Report()
	{
		// The string that will hold the report data.
		string report = "";
		
		// Track total instances.
		int total = 0;
		
		// Append instances.
		foreach (KeyValuePair<GameObject, int> pair in instanceCounts)
		{
			string name = pair.Key.name;
			int count = pair.Value;
			
			total += count;
			
			report += name + "\t\t\t\t\t\t\t\t" + count + "\n";
		}
		
		// Add the total.
		report += "\nTotal = " + total;
		
		// Log the report.
		Debug.Log(report);
	}
	
	
	// Private Methods
	// ----------------------------------------------------------------------------
	
	/** Create a game object. */
	private GameObject CreateObject(GameObject template, bool active = true, bool staticBatch = false, string name = null)
	{
		// Create a new object instance.
		GameObject o = (GameObject) GameObject.Instantiate(template);
		o.name = name ?? template.name;

        // Combine for static batching if required.
		if (staticBatch)
			StaticBatchingUtility.Combine(o);
		
		// Deactivate if required.
		if (!active)
			o.SetActive(false);

        // Remember that object was created by pool.
        pooledLookup[o] = true;

        return o;
	}
	
	/** Get a list of available objects for the specified object name. */
	private Stack<GameObject> GetObjectList(string name)
	{
		// The list of objects.
		Stack<GameObject> objects;
		
		// Create a new object list if it doesn't exist yet.
		if (!objectLists.ContainsKey(name))
		{
			objects = new Stack<GameObject>();
			objectLists.Add(name, objects);
		}
		
		// Otherwise, find the mapped object list.
		else
			objects = objectLists[name];
		
		return objects;
	}
}
