using UnityEngine;
using System.Collections;

public class widgetChevronWarnings : MonoBehaviour
{
    public graphicsSlicedMesh slicer;
    private graphicsSlicedMesh thisSlicer;
    public Renderer r;
    private Material m;
    private Renderer parentR;
    public bool bottom;

    void SetWidth()
    {
        float width = slicer.Width - (slicer.Border * 2f);

        thisSlicer.Width = width;
        m.SetTextureScale("_MainTex", new Vector2(width * 8f, 1));

        float height = thisSlicer.Height;

        float yPos = (slicer.Height * 0.5f) + (height * 0.5f);

        if (!bottom)
            transform.localPosition = new Vector3(0, yPos, 0.02f);
        else
            transform.localPosition = new Vector3(0, -yPos, 0.02f);
    }

	void Start ()
    {
        if (!r)
            r = GetComponent<Renderer>();
        m = r.material;

        if (!slicer)
            slicer = transform.parent.gameObject.GetComponent<graphicsSlicedMesh>();

        parentR = transform.parent.gameObject.GetComponent<Renderer>();

        thisSlicer = GetComponent<graphicsSlicedMesh>();
        SetWidth();
	}

    void Update ()
    {
        m.SetColor("_TintColor", parentR.material.GetColor("_TintColor"));
    }
}
