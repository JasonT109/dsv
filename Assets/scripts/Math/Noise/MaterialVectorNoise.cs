using UnityEngine;
using System.Collections;

public class MaterialVectorNoise : MonoBehaviour
{

    public SmoothNoise NoiseSourceX;
    public SmoothNoise NoiseSourceY;
    public SmoothNoise NoiseSourceZ;
    public SmoothNoise NoiseSourceW;

    public Material Material
        { get; private set; }

    public string Parameter;

    private void Start()
    {
        NoiseSourceX.Start();
        NoiseSourceY.Start();
        NoiseSourceZ.Start();
        NoiseSourceW.Start();

        Material = GetComponent<Renderer>().material;
    }

    /** Updating. */
    private void Update()
    {
        if (!Material)
            return;

        var x = NoiseSourceX.Update();
        var y = NoiseSourceY.Update();
        var z = NoiseSourceZ.Update();
        var w = NoiseSourceW.Update();

        Material.SetVector(Parameter, new Vector4(x, y, z, w));
    }
}
