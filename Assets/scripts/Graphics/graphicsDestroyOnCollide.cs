using UnityEngine;
using System.Collections;

public class graphicsDestroyOnCollide : MonoBehaviour
{
    public bool destroyOnDistance = false;
    public float destroyDistance = 25f;

    void OnTriggerEnter()
    {
        if (!destroyOnDistance)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (destroyOnDistance)
        {
            GameObject g = GameObject.FindWithTag("ServerData");
            if (g != null)
            {
                float d = Vector3.Distance(g.transform.position, transform.position);
                if (d > destroyDistance)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
