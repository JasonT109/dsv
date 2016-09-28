using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject Prefab;
    public float Interval = 1;

    public float MinDistance = 0;

    public bool Pooled;
    public bool Siblings;
    public bool InheritScale;

    private Graphic _graphic;


    private Vector3 _lastSpawnPosition;

	void OnEnable()
	{
        _lastSpawnPosition = transform.localPosition;
	    _graphic = GetComponentInChildren<Graphic>();

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
            var go = Pooled ? ObjectPool.Get(Prefab) : Instantiate(Prefab);
            if (!go)
                yield break;

            if (Siblings)
                go.transform.SetParent(transform.parent, false);

            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;

            if (InheritScale)
                go.transform.localScale = transform.localScale;

            // Inherit color from spawner.
            if (_graphic)
                ApplyColor(go);
        }
    }

    private void ApplyColor(GameObject go)
    {
        var g = go.GetComponentInChildren<Graphic>();
        g.color = _graphic.color;
    }

}
