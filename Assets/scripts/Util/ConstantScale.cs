using UnityEngine;
using System.Collections;

public class ConstantScale : MonoBehaviour
{
    public Transform Root;

    private Vector3 _initialScale;
    private Vector3 _initialRootScale;

	void Start()
	{
	    _initialScale = transform.localScale;
        _initialRootScale = Root.localScale;
	}
	
	void LateUpdate()
	{
	    var s = _initialRootScale.x / Root.localScale.x;
        transform.localScale = _initialScale * s;
    }
}
