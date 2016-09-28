using UnityEngine;
using System.Collections;

public static class GameObjectExtensions
{

    public static void Cleanup(this GameObject go, float delay = 0.5f)
    {
        if (!go)
            return;

        go.SetActive(false);
        Object.Destroy(go, delay);
    }

}
