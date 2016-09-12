using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ConstantScale : MonoBehaviour
{
    public Transform Root;
    public float Scale = 1;

    private Vector3 _initialScale;
    private Vector3 _initialRootScale;

	void Start()
	{
        if (!Root)
        {
            var gesture = transform.GetComponentInParents<TransformGesture>();
            Root = gesture ? gesture.transform : transform;
        }

        _initialScale = transform.localScale * Scale;
        _initialRootScale = Root.localScale;
	}
	
	void LateUpdate()
	{
	    var s = _initialRootScale.x / Root.localScale.x;
        transform.localScale = _initialScale * s;
    }
}
