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

    public float scaleTweakFactor = 0.95f;

    public bool resetWidow = false;

    private BoxCollider boxCollider;
    private float defaultWidth = 190f;
    private float defaultHeight = 106.875f;
    private Vector3[] rPositions;
    private Vector2[] sScales;
    private float currentXScale = 1;
    private float currentYScale = 1;
    private graphicsSlicedMesh slicer;
    private graphicsSlicedMesh titleBarSlicer;
    private Vector3[] childPositions = { new Vector3(-94.96f, 53.42f, 0f), new Vector3(-94.99f, -53.35f, 0f), new Vector3(0, 53.43f, 0f) };

    public void ResetWindowSize()
    {
        //SetWindowSize(defaultWidth, defaultHeight);
        windowWidth = defaultWidth;
        windowHeight = defaultHeight;
        resetWidow = false;

        for (int i = 0; i < repositionItems.Length; i++)
        {
            repositionItems[i].transform.localPosition = childPositions[i];
        }
    }

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
            if (slicedContent[i])
            {
                slicedContent[i].Width = width;
                slicedContent[i].Height = height;
            }

        }

        titleBarSlicer.Width = width;
        titleBarSlicer.Height = height;

        titleBarSlicer.Border = slicer.Border;
        titleBarSlicer.Margin = slicer.Margin;

        currentXScale = (slicer.Width / defaultWidth);
        currentYScale = (slicer.Height / defaultHeight);


        for (int i = 0; i < repositionItems.Length; i++)
        {
            repositionItems[i].transform.localPosition = new Vector3((rPositions[i].x * currentXScale), (rPositions[i].y * currentYScale));
        }

        for (int i = 0; i < scaleItems.Length; i++)
        {
            scaleItems[i].transform.localScale = new Vector3((sScales[i].x * currentXScale) * scaleTweakFactor, (sScales[i].y * currentYScale) * scaleTweakFactor, 1);
        }
    }

    void Awake ()
    {
        slicer = transform.GetComponent<graphicsSlicedMesh>();
        titleBarSlicer = titleBar.GetComponent<graphicsSlicedMesh>();

        boxCollider = GetComponent<BoxCollider>();

        rPositions = new Vector3[repositionItems.Length];

        for (int i = 0; i < repositionItems.Length; i++)
        {
            rPositions[i] = childPositions[i];
        }

        sScales = new Vector2[scaleItems.Length];

        for (int i = 0; i < scaleItems.Length; i++)
        {
            sScales[i] = new Vector2(1,1);
        }
    }

    void Start ()
    {
        SetWindowSize(windowWidth, windowHeight);
    }

    void Update ()
    {
        SetWindowSize(windowWidth, windowHeight);

        if (resetWidow)
        {
            ResetWindowSize();
        }
    }
}
