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

        // Determine current brightness.
        var b = serverUtils.GetServerData("camerabrightness");

        // On the server or debug screen, always use a brightness of 100%.
        var isServer = serverUtils.IsServer();
        var isDebug = serverUtils.IsInDebugScreen();
        if (isServer || isDebug)
            b = 1f;

        _effectMaterial.SetFloat("_Brightness", b);

        if (!isDebug)
        {
            if (Input.GetKeyDown(KeyCode.Equals))
                localBrightness += 0.05f;
            if (Input.GetKeyDown(KeyCode.Minus))
                localBrightness -= 0.05f;
        }

        _effectMaterial.SetFloat("_LocalBrightness", Mathf.Clamp(localBrightness, 0, 2));
    }
}
