using UnityEngine;
using System.Collections;

public class NavMapTerrain : Singleton<NavMapTerrain>
{
    public Transform Root;
    public Transform Origin;
    public Terrain Terrain;
    public GameObject Water;
    public GameObject Acid;
}
