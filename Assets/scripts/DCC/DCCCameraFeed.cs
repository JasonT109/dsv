using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class DCCCameraFeed : MonoBehaviour
{
    
    public enum sizes
    {
        small,
        medium,
        large
    }
    public enum positions
    {
        largeCentre,
        midLeft,
        midRight,
        small1,
        small2,
        small3,
        small4,
        small5,
        small6,
        small7,
        small8,
        small9,
        small10
    }

    [Header("Appearance")]
    //public sizes size = sizes.medium;
    public positions position = positions.midLeft;
    [Range(0, 9)]
    public int feedMaterialID;
    public int materialID
    {
        get
        {
            return feedMaterialID;
        }
        set
        {
            feedMaterialID = value;
            SetMaterial(feedMaterialID);
        }
    }
    public string titleText = "Camera feed";

    [Header ("Required Components")]
    public Transform videoPlane;
    public Transform titleBox;
    public widgetText title;
    public graphicsSlicedMesh frame;

    [Header("Initial Setup")]
    public Vector2[] frameSizes = new Vector2[3];
    public float[] windowScales = new float[3];
    public Vector3[] windowPositions = new Vector3[3];
    public float midOffset = 200f;
    public float smallOffsetX = 100f;
    public float smallOffsetY = 100f;
    public Material[] materials = new Material[10];

    public void SetPosition()
    {
        switch (position)
        {
            case positions.largeCentre:
                transform.localPosition = windowPositions[0];
                titleBox.localPosition = new Vector3(0, 0, 0);
                SetSize(sizes.large);
                break;
            case positions.midLeft:
                transform.localPosition = windowPositions[1];
                titleBox.localPosition = new Vector3(0, 0, 0);
                SetSize(sizes.medium);
                break;
            case positions.midRight:
                transform.localPosition = new Vector3 (windowPositions[1].x + midOffset, windowPositions[1].y, windowPositions[1].z);
                titleBox.localPosition = new Vector3(0, 0, 0);
                SetSize(sizes.medium);
                break;
            case positions.small1:
                transform.localPosition = windowPositions[2];
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small2:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX, 0, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small3:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 2, 0, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small4:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 3, 0, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small5:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 4, 0, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small6:
                transform.localPosition = windowPositions[2] + new Vector3(0, smallOffsetY, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small7:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX, smallOffsetY, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small8:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 2, smallOffsetY, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small9:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 3, smallOffsetY, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
            case positions.small10:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 4, smallOffsetY, 0);
                titleBox.localPosition = new Vector3(-7.6f, 0, 0);
                SetSize(sizes.small);
                break;
        }
    }

    void SetSize(sizes newSize)
    {
        switch (newSize)
        {
            case sizes.large:
                titleBox.localScale = Vector3.one * windowScales[0];
                videoPlane.localScale = Vector3.one * windowScales[0];
                frame.Width = frameSizes[0].x;
                frame.Height = frameSizes[0].y;
                break;
            case sizes.medium:
                titleBox.localScale = Vector3.one * windowScales[1];
                videoPlane.localScale = Vector3.one * windowScales[1];
                frame.Width = frameSizes[1].x;
                frame.Height = frameSizes[1].y;
                break;
            case sizes.small:
                titleBox.localScale = Vector3.one * windowScales[2];
                videoPlane.localScale = Vector3.one * windowScales[2];
                frame.Width = frameSizes[2].x;
                frame.Height = frameSizes[2].y;
                break;
        }
    }

    void SetMaterial(int ID)
    {
        if (materials[ID])
        {
            Renderer r = videoPlane.GetComponentInChildren<Renderer>();
            r.material = materials[ID];
        }
    }

    void SetTitle(string newTitle)
    {
        title.Text = newTitle;
    }

	void Start ()
    {
        Update();
        
    }

	void Update ()
    {
        SetPosition();
        SetTitle(titleText + " " + (feedMaterialID + 1));
    }
}
