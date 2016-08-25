using UnityEngine;
using System.Collections;
using Meg.Networking;

public class CameraBrightness : MonoBehaviour {

    public Material EffectMaterial;
    public float localBrightness = 1;

    private Material _effectMaterial;

    void EnsureMaterialExists()
    {
        if (!_effectMaterial)
            _effectMaterial = new Material(EffectMaterial);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        EnsureMaterialExists();
        Graphics.Blit(src, dst, _effectMaterial);
    }

    void Update()
    {
        EnsureMaterialExists();

        var b = serverUtils.GetServerData("camerabrightness");
        _effectMaterial.SetFloat("_Brightness", b);

        if (Input.GetKeyDown(KeyCode.Equals))
            localBrightness += 0.05f;
        if (Input.GetKeyDown(KeyCode.Minus))
            localBrightness -= 0.05f;

        _effectMaterial.SetFloat("_LocalBrightness", Mathf.Clamp(localBrightness, 0, 2));
    }
}
