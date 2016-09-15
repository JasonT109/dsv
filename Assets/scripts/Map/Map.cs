using UnityEngine;
using System.Collections;

public class Map : Singleton<Map>
{

    // Components
    // ------------------------------------------------------------

    [Header("Components")]
    public Transform Root;
    public Transform Origin;

    [Header("Elements")]
    public Terrain Terrain;
    public GameObject Water;
    public GameObject Acid;

    [Header("Camera")]
    public Transform CameraRoot;
    public Transform CameraPitch;
    public Camera Camera;


    // Properties
    // ------------------------------------------------------------

    [Header("Mapping")]

    /** Scaling factor for converting vessel space into 3d map space. */
    public Vector3 VesselToMapScale = new Vector3(45f, 0.04f, 45f);

    /** Maximum depth that a vessel can attain on this map. */
    public float VesselMaxMapDepth = 12000f;


    // Public Methods
    // ------------------------------------------------------------

    /** Convert a point in vessel space to 3d map space. */
    public Vector3 VesselToMapSpace(Vector3 p)
    {
        var o = Origin.position;
        return new Vector3(
            o.x + p.x * VesselToMapScale.x,
            o.y + (VesselMaxMapDepth - p.z) * VesselToMapScale.y,
            o.z + p.y * VesselToMapScale.z);
    }

    /** Convert a normalized map-space 3d position into world space. */
    public Vector3 MapToVesselSpace(Vector3 p)
    {
        var o = Origin.position;
        return new Vector3(
            (p.x - o.x) / VesselToMapScale.x,
            (p.z - o.z) / VesselToMapScale.z,
            VesselMaxMapDepth - (p.y - o.y) / VesselToMapScale.y);
    }


}
