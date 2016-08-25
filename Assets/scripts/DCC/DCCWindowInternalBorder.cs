using UnityEngine;
using System.Collections;

public class DCCWindowInternalBorder : MonoBehaviour
{
    public bool useInternalBorder = true;
    public Texture2D border1;
    public Texture2D border2;
    public Texture2D border3;
    public Texture2D transparent;

    public float border1threshold = 90f;
    public float border2threshold = 30f;

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
        if (useInternalBorder)
        {
            if (GetComponent<graphicsSlicedMesh>().Width >= border1threshold)
                m.mainTexture = border1;
            else if (GetComponent<graphicsSlicedMesh>().Width < border1threshold && GetComponent<graphicsSlicedMesh>().Width > border2threshold)
                m.mainTexture = border2;
            else
                m.mainTexture = border3;
        }
        else
        {
            m.mainTexture = transparent;
        }

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
