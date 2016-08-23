﻿using UnityEngine;
using System.Collections;
using Meg.Networking;

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
            if (value != materialID)
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

    [Header("Initial Setup")]
    public Vector2[] frameSizes = new Vector2[3];
    public float[] windowScales = new float[3];
    public Vector3[] windowPositions = new Vector3[3];
    public float midOffset = 200f;
    public float smallOffsetX = 100f;
    public float smallOffsetY = 100f;
    public Material[] materials = new Material[10];


    private Color fadecolor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
    private Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color lerpColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private float lerpValue = 0;
    private bool isLerping = false;
    private Renderer r;

	[Header("Live feed")]
	public LiveCameraOutputFinder Outputs;
	public GameObject LivePlane;
	public GameObject FakePlane;
	//public MeshRenderer _mesh;
	//public AVProLiveCamera[] _liveCameras;


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
                GetComponent<BoxCollider>().size = frameSizes[0];
                break;
            case sizes.medium:
                titleBox.localScale = Vector3.one * windowScales[1];
                videoPlane.localScale = Vector3.one * windowScales[1];
                frame.Width = frameSizes[1].x;
                frame.Height = frameSizes[1].y;
                GetComponent<BoxCollider>().size = frameSizes[1];
                break;
            case sizes.small:
                titleBox.localScale = Vector3.one * windowScales[2];
                videoPlane.localScale = Vector3.one * windowScales[2];
                frame.Width = frameSizes[2].x;
                frame.Height = frameSizes[2].y;
                GetComponent<BoxCollider>().size = frameSizes[2];
                break;
        }
    }

    void SetMaterial(int ID)
    {
		if(!Outputs.isLive || ID > Outputs.getNumCams()-1)
		{
        	if (materials[ID])
        	{
        	    //works
				Renderer r = videoPlane.GetComponentInChildren<Renderer>();
				//Renderer r = FakePlane.GetComponentInChildren<Renderer>();
        	    r.material = materials[ID];
        	    isLerping = true;
        	    lerpValue = 0;
				
        	    if (position == positions.midLeft && !isTopScreen)
        	    {
        	        window.commsContent = ID;
        	        serverUtils.PostServerData("dcccommscontent", ID);
        	    }
        	}
		}
		else
		{
			if (Outputs.GetOutput(ID) != null && Outputs.GetOutput(ID).OutputTexture != null)
			{
				ApplyMapping(Outputs.GetOutput(ID).OutputTexture);
			}
		}
    }

    void SetTitle(string newTitle)
    {
        title.Text = newTitle;
    }

	void Start ()
    {
		//TODO if live camera valid, then video plane = liveplane
		//else video plane = fake plane
		//link up all video planes in inspector



	}

	void Awake()
	{
		if(materialID < 4 && Outputs.isLive)
		{
			if(materialID == 0)
			{
				FakePlane.SetActive(false);
				LivePlane.SetActive(true);
				r = LivePlane.GetComponentInChildren<Renderer>();
			}

			if(Outputs.getNumCams() > 1 && materialID == 1)
			{
				FakePlane.SetActive(false);
				LivePlane.SetActive(true);
				r = LivePlane.GetComponentInChildren<Renderer>();
			}

			if(Outputs.getNumCams() > 2 && materialID == 2)
			{
				FakePlane.SetActive(false);
				LivePlane.SetActive(true);
				r = LivePlane.GetComponentInChildren<Renderer>();
			}

			if(Outputs.getNumCams() > 3 && materialID == 3)
			{
				FakePlane.SetActive(false);
				LivePlane.SetActive(true);
				r = LivePlane.GetComponentInChildren<Renderer>();
			}
		}
		else
		{
			if(materialID < 4)
			{
				FakePlane.SetActive(true);
				LivePlane.SetActive(false);
			}
			r = videoPlane.GetComponentInChildren<Renderer>();
		}

		window = GetComponentInParent<DCCWindow>();
		Update();
	}

	//void Awake()
	//{
	//	if(Outputs.isLive)
	//	{
	//		for(int i = 0; i < Outputs.getNumCams(); ++i)
	//		{
	//			SetMaterial(i);
	//		}
	//	}
	//}

	void Update ()
    {
        SetPosition();
        SetTitle(titleText + " " + (feedMaterialID + 1));

        if (isLerping)
        {
            lerpValue += Time.deltaTime;
            lerpColor = Color.Lerp(fadecolor, defaultColor, Mathf.Clamp01(lerpValue));

            //Renderer r = videoPlane.GetComponentInChildren<Renderer>();
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
