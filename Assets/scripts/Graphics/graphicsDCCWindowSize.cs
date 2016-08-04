using UnityEngine;
using System.Collections;

public class graphicsDCCWindowSize : MonoBehaviour
{
    private graphicsSlicedMesh parentSlicer;
    private graphicsSlicedMesh slicer;

	void Start ()
    {
        parentSlicer = transform.parent.GetComponent<graphicsSlicedMesh>();
        slicer = GetComponent<graphicsSlicedMesh>();
	}

	void Update ()
    {
        slicer.Height = parentSlicer.Height;
        slicer.Width = parentSlicer.Width;
        slicer.Border = parentSlicer.Border;
        slicer.Margin = parentSlicer.Margin;
	}
}
