using UnityEngine;
using System.Collections;

public class ReciprocalScale : MonoBehaviour
{

    private Vector3 _initialScale;

    public Transform Target;

    private void Start()
    {
        _initialScale = transform.localScale;

        if (!Target && Camera.main)
            Target = Camera.main.transform;

        UpdateScale();
    }

	private void LateUpdate ()
        { UpdateScale(); }

    private void UpdateScale()
    {
        if (!Target)
            return;

        var s = Target.localScale;
        if (Mathf.Approximately(s.x, 0) || Mathf.Approximately(s.y, 0) || Mathf.Approximately(s.z, 0))
            return;

        var o = _initialScale;
        transform.localScale = new Vector3(o.x / s.x, o.y / s.y, o.z / s.z);
    }

}
