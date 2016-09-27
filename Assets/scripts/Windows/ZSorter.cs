using UnityEngine;
using System.Collections;

public class ZSorter : MonoBehaviour
{

    /** Z-offset between successive children. */
    public float Offset;


    private void Update()
    {
        var n = transform.childCount;
        for (var i = 0; i < n; i++)
        {
            var child = transform.GetChild(i);
            var p = child.transform.localPosition;
            p.z = (i * Offset);
            child.transform.localPosition = p;
        }
    }

}
