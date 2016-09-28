using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class ConstantScale : MonoBehaviour
{
    public Transform Root;
    public float InitialRootScale;

    private Vector3 _initialScale;

	void Start()
	{
        if (!Root)
        {
            var gesture = transform.GetComponentInParents<TransformGesture>();
            Root = gesture ? gesture.transform : transform;
        }

        _initialScale = transform.localScale;
        if (InitialRootScale <= 0)
            InitialRootScale = Root.localScale.x;
    }

    void LateUpdate()
	{
	    var s = InitialRootScale / Root.localScale.x;
        transform.localScale = _initialScale * s;
    }
}
