using UnityEngine;
using Meg.Maths;
using Meg.Networking;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

[RequireComponent(typeof(VectorLine))]
public class MapGrid : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Tiling factor for this grid layer. */
    public float Tiling = 20;


    // Members
    // ------------------------------------------------------------

    /** Renderer. */
    private Renderer _renderer;


    // Unity Methods
    // ------------------------------------------------------------

    /** Startup. */
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetTextureScale("_MainTex", Vector2.one * Tiling);
    }
    
}
