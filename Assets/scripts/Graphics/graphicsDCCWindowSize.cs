using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class graphicsDCCWindowSize : MonoBehaviour
{
    [Header("Child objects:")]
    public GameObject titleBar;
    public graphicsSlicedMesh[] slicedContent;
    public GameObject[] repositionItems;
    public GameObject[] scaleItems;

    [Header("Size:")]
    public float windowWidth = 190f;
    public float windowHeight = 106.875f;

    private float _scale = 1;
    public float windowScale
    {
        get
        {
            return _scale;
        }
        set
        {
            _scale = value;
            windowWidth = defaultWidth * _scale;
            windowHeight = defaultHeight * _scale;
            SetWindowSize(windowWidth, windowHeight);
        }
    }

    private Vector3 _windowPosition;
    public Vector3 windowPosition
    {
        get
        {
            return _windowPosition;
        }
        set
        {
            _windowPosition = value;
            SetWindowPosition(_windowPosition);
        }
    }

    private BoxCollider boxCollider;
    private float defaultWidth = 190f;
    private float defaultHeight = 106.875f;
    private Vector2[] rPositions;
    private Vector2[] sScales;
    private float currentXScale = 1;
    private float currentYScale = 1;
    private graphicsSlicedMesh slicer;
    private graphicsSlicedMesh titleBarSlicer;


    public void SetWindowPosition(Vector3 newWindowPosition)
    {
        transform.position = newWindowPosition;
    }

    public void SetWindowSize(float width, float height)
    {
        boxCollider.size = new Vector3(width, height, 1);
        slicer.Width = width;
        slicer.Height = height;

        for (int i = 0; i < slicedContent.Length; i++)
        {
            slicedContent[i].Width = width;
            slicedContent[i].Height = height;
            //slicedContent[i].Border = slicer.Border;
            //slicedContent[i].Margin = slicer.Margin;
        }

        titleBarSlicer.Width = width;
        titleBarSlicer.Height = height;

        titleBarSlicer.Border = slicer.Border;
        titleBarSlicer.Margin = slicer.Margin;

        currentXScale = (slicer.Width) / (defaultWidth);
        currentYScale = (slicer.Height) / (defaultHeight);


        for (int i = 0; i < repositionItems.Length; i++)
        {
            repositionItems[i].transform.localPosition = new Vector2((rPositions[i].x * currentXScale), (rPositions[i].y * currentYScale));
        }

        for (int i = 0; i < scaleItems.Length; i++)
        {
            scaleItems[i].transform.localScale = new Vector3(sScales[i].x * currentXScale, sScales[i].y * currentYScale, 1);
        }

    }

    void Awake ()
    {
        slicer = transform.GetComponent<graphicsSlicedMesh>();
        titleBarSlicer = titleBar.GetComponent<graphicsSlicedMesh>();

        boxCollider = GetComponent<BoxCollider>();

        rPositions = new Vector2[repositionItems.Length];

        for (int i = 0; i < repositionItems.Length; i++)
        {
            rPositions[i] = repositionItems[i].transform.localPosition;
        }

        sScales = new Vector2[scaleItems.Length];

        for (int i = 0; i < scaleItems.Length; i++)
        {
            sScales[i] = scaleItems[i].transform.localScale;
        }
    }
#if UNITY_EDITOR
    void Update ()
    {
        SetWindowSize(windowWidth, windowHeight);
    }
#endif
}
