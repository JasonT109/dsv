using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

    public float Interval = 1;

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(Interval);
        gameObject.Cleanup();
    }

}
