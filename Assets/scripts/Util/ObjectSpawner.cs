using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject Prefab;
    public float Interval = 1;

    public float MinDistance = 0;

    public bool Siblings;


    private Vector3 _lastSpawnPosition;

	void OnEnable()
	{
        _lastSpawnPosition = transform.localPosition;
        StopAllCoroutines();
	    StartCoroutine(SpawnRoutine());
	}

    IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(Interval);
        yield return wait;

        while (gameObject.activeSelf)
        {
            while (Vector3.Distance(transform.localPosition, _lastSpawnPosition) < MinDistance)
                yield return wait;

            _lastSpawnPosition = transform.localPosition;
            var go = Instantiate(Prefab);
            if (!go)
                yield break;

            if (Siblings)
                go.transform.SetParent(transform.parent, false);

            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
        }
    }

}
