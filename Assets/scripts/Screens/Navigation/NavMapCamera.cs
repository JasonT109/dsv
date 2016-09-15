using UnityEngine;
using System.Collections;

public class NavMapCamera : Singleton<NavMapCamera>
{
    [Header("Map Components")]
    public Transform Root;
    public Transform Pitch;
    public Camera Camera;
}
