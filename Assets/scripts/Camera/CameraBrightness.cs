using UnityEngine;
using System.Collections;
using Meg.Networking;

public class CameraBrightness : MonoBehaviour {

    public Material EffectMaterial;
    public float localBrightness = 1;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, EffectMaterial);
    }

    void Update ()
    {
        
        //if (serverUtils.IsServer())
            //return;
        
        var b = serverUtils.GetServerData("camerabrightness");
        EffectMaterial.SetFloat("_Brightness", b);

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            localBrightness += 0.05f;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            localBrightness -= 0.05f;
        }

        EffectMaterial.SetFloat("_LocalBrightness", Mathf.Clamp(localBrightness, 0, 2));
    }
}
