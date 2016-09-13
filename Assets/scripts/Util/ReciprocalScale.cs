using UnityEngine;
using System.Collections;

public class ReciprocalScale : MonoBehaviour
{

    public Transform Target;

    private void Start()
    {
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

        transform.localScale = new Vector3(1 / s.x, 1 / s.y, 1 / s.z);
    }

}
