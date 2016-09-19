using UnityEngine;
using System.Collections;
using Meg.Networking;

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
        midLeftLow,
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
            if (value != materialID || r.material.mainTexture == null)
            {
                feedMaterialID = value;
                SetMaterial(feedMaterialID);
            }
        }
    }
    public string titleText = "Camera feed";
    public bool isTopScreen = false;

    [Header ("Required Components")]
    public Transform videoPlane;
    public Transform titleBox;
    public widgetText title;
    public graphicsSlicedMesh frame;
    public DCCWindow window;
    public Transform overlay1;

    [Header("Initial Setup")]
    public Vector2[] frameSizes = new Vector2[3];
    public float[] windowScales = new float[3];
    public Vector3[] windowPositions = new Vector3[3];
    public Vector3[] TitleOffset = new Vector3[3];
    public float midOffsetX = 200f;
    public float midOffsetY = -10f;
    public float smallOffsetX = 100f;
    public float smallOffsetY = 100f;
    public Material[] materials = new Material[10];

    [Header("Live feed")]
    public LiveCameraOutputFinder Outputs;

    private Color fadecolor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
    private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color lerpColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private float lerpValue = 0;
    private bool isLerping = false;
    private Renderer r;

    public void SetPosition()
    {
        switch (position)
        {
            case positions.largeCentre:
                transform.localPosition = windowPositions[0];
                titleBox.localPosition = TitleOffset[0];
                SetSize(sizes.large);
                break;
            case positions.midLeft:
                transform.localPosition = windowPositions[1];
                titleBox.localPosition = TitleOffset[1];
                SetSize(sizes.medium);
                break;
            case positions.midLeftLow:
                transform.localPosition = new Vector3 (windowPositions[1].x, windowPositions[1].y + midOffsetY, windowPositions[1].z);
                titleBox.localPosition = TitleOffset[1];
                SetSize(sizes.medium);
                break;
            case positions.midRight:
                transform.localPosition = new Vector3 (windowPositions[1].x + midOffsetX, windowPositions[1].y, windowPositions[1].z);
                titleBox.localPosition = TitleOffset[1];
                SetSize(sizes.medium);
                break;
            case positions.small1:
                transform.localPosition = windowPositions[2];
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small2:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX, 0, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small3:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 2, 0, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small4:
                transform.localPosition = windowPositions[2] + new Vector3(0, smallOffsetY, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small5:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX , smallOffsetY, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small6:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 2, smallOffsetY, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small7:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX, smallOffsetY, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small8:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 2, smallOffsetY, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small9:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 3, smallOffsetY, 0);
                titleBox.localPosition = TitleOffset[2];
                SetSize(sizes.small);
                break;
            case positions.small10:
                transform.localPosition = windowPositions[2] + new Vector3(smallOffsetX * 4, smallOffsetY, 0);
                titleBox.localPosition = TitleOffset[2];
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
                GetComponent<BoxCollider>().size = frameSizes[0];
                if (overlay1)
                    overlay1.localScale = Vector3.one * windowScales[0];
                break;
            case sizes.medium:
                titleBox.localScale = Vector3.one * windowScales[1];
                videoPlane.localScale = Vector3.one * windowScales[1];
                frame.Width = frameSizes[1].x;
                frame.Height = frameSizes[1].y;
                GetComponent<BoxCollider>().size = frameSizes[1];
                if (overlay1)
                    overlay1.localScale = Vector3.one * windowScales[1];
                break;
            case sizes.small:
                titleBox.localScale = Vector3.one * windowScales[2];
                videoPlane.localScale = Vector3.one * windowScales[2];
                frame.Width = frameSizes[2].x;
                frame.Height = frameSizes[2].y;
                GetComponent<BoxCollider>().size = frameSizes[2];
                if (overlay1)
                    overlay1.localScale = Vector3.one * windowScales[2];
                break;
        }
    }

    void SetMaterial(int ID)
    {

        if (!Outputs || !Outputs.isLive)
            Debug.Log("AVPro offline...");

		if(!Outputs || (!Outputs.isLive || ID > Outputs.getNumCams()-1))
		{
        	if (materials[ID])
        	{
				r = videoPlane.GetComponentInChildren<Renderer>();
        	    r.material = materials[ID];
        	}
		}
		else
        {
            if (Outputs.GetOutput(ID) != null && Outputs.GetOutput(ID).OutputTexture != null)
            {
				ApplyMapping(Outputs.GetOutput(ID).OutputTexture);
                Debug.Log("Applying live feed texture of ID: " + ID + " to: " + gameObject);
            }
		}

        isLerping = true;
        lerpValue = 0;

        if (position == positions.midLeft && !isTopScreen)
        {
            window.commsContent = ID;
            //serverUtils.PostServerData("dcccommscontent", ID);
        }
    }

    void SetTitle(string newTitle)
    {
        title.Text = newTitle;
    }

	void Start()
	{
        if (!Outputs)
            Outputs = ObjectFinder.Find<LiveCameraOutputFinder>();

        window = GetComponentInParent<DCCWindow>();
        r = videoPlane.GetComponentInChildren<Renderer>();
        SetMaterial(feedMaterialID);
        Update();
	}

	void Update ()
    {
        if (r.material.mainTexture == null)
            SetMaterial(feedMaterialID);

        SetPosition();
        SetTitle(titleText + " " + (feedMaterialID + 1));

        if (isLerping)
        {
            lerpValue += Time.deltaTime;
            lerpColor = Color.Lerp(fadecolor, defaultColor, Mathf.Clamp01(lerpValue));
            r.material.SetColor("_TintColor", lerpColor);

            if (lerpValue > 1)
            {
                isLerping = false;
                lerpValue = 0;
            }
        }
        if (isTopScreen)
        {
            int feed = (int)serverUtils.GetServerData("dcccommscontent");
            if (feed != materialID)
                materialID = feed;
        }
    }

	private void ApplyMapping(Texture texture)
	{
        if (r != null)
		{
			foreach (Material m in r.materials)
			{
				m.mainTexture = texture;
			}
		}
	}
}
