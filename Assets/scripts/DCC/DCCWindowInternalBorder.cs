using UnityEngine;
using System.Collections;

public class DCCWindowInternalBorder : MonoBehaviour
{
    public Texture2D border1;
    public Texture2D border2;

    public Renderer r;
    private Material m;

    void Awake ()
    {
        if (!r)
            r = GetComponent<Renderer>();

        if (!m)
            m = r.material;
    }

    void SetTexture()
    {
        if (GetComponent<graphicsSlicedMesh>().Width > 90f)
            m.mainTexture = border1;
        else
            m.mainTexture = border2;
    }

	void Start ()
    {
        SetTexture();
	}
	
	void Update ()
    {
        SetTexture();
    }
}
