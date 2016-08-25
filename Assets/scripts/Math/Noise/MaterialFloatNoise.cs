using UnityEngine;
using System.Collections;

public class MaterialFloatNoise : MonoBehaviour
{

    public SmoothNoise NoiseSource;

    public Material Material
        { get; private set; }

    public string Parameter;

    private void Start()
    {
        NoiseSource.Start();
        Material = GetComponent<Renderer>().material;
    }

    /** Updating. */
    private void Update()
    {
        NoiseSource.Update();
        if (Material)
            Material.SetFloat(Parameter, NoiseSource.Value);
    }
}
