using UnityEngine;
using System.Collections;

public class DCCEnableWindowContent : MonoBehaviour
{
    
    /** The type of content we're enabling. */
    public DCCWindow.contentID Content;

    /** The object to enable or disable. */
    public GameObject Target;


    /** Initialization. */
	private void Start()
	    { UpdateTarget(); }

    /** Updating. */
    private void Update()
        { UpdateTarget(); }

    /** Update enabled state. */
    private void UpdateTarget()
    {
        if (DCCScreenManager.Instance)
            Target.SetActive(DCCScreenManager.Instance.IsContentVisible(Content));
    }

}
