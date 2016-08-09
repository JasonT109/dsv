using UnityEngine;
using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Networking;
using Meg.DCC;

public class DCCQuadButtonHilight : MonoBehaviour
{
    [Header("Screen content")]
    public DCCWindow.contentID screenContent;

    [Header ("Quad menu objects")]
    public GameObject[] quadDirections;
    public GameObject quadDirection;

    [Header("Color Set-up")]
    public Color unhilighted = Color.gray;
    public Color highlighted = Color.red;
    public float hilightSpeed = 0.2f;

    [Header("Selected Direction")]
    public DCCScreenContentPositions.positionID selectedDirection = DCCScreenContentPositions.positionID.hidden;

    private Vector3 transformDirection;
    private Color topLeftColor = Color.gray;
    private Color topRightColor = Color.gray;
    private Color bottomLeftColor = Color.gray;
    private Color bottomRightColor = Color.gray;

    private float topLeftTime = 0f;
    private float topRightTime = 0f;
    private float bottomLeftTime = 0f;
    private float bottomRightTime = 0f;

    private void OnEnable()
    {
        GetComponent<TransformGesture>().Transformed += transformHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
    }

    private void OnDisable()
    {
        GetComponent<TransformGesture>().Transformed -= transformHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
    }

    private void transformHandler(object sender, EventArgs e)
    {
        var gesture = sender as TransformGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        transformDirection = gesture.DeltaPosition;
        quadDirection.transform.localPosition += transformDirection;
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        if (selectedDirection != DCCScreenContentPositions.positionID.hidden)
        {
            //get where we are coming from, we can then tell destination content to occupy this spot (swap positions)
            string newPosition = GetContentsPosition(screenContent);
            SetServerScreenContent(newPosition);
        }

        transformDirection = Vector3.zero;
        quadDirection.transform.localPosition = Vector3.zero;
    }

    void RepositionContentAtDestination(DCCScreenContentPositions.positionID targetPosition, string newPosition)
    {
        string[] quads = new string[] { "DCCquadScreen0", "DCCquadScreen1", "DCCquadScreen2", "DCCquadScreen3", "DCCquadScreen4" };

        DCCWindow.contentID targetContent = (DCCWindow.contentID)serverUtils.GetServerData(quads[(int)targetPosition]);

        if (newPosition != "hidden")
        {
            //move this content targetContent to the quadbox newPosition
            serverUtils.PostServerData(newPosition, (float)targetContent);

            Debug.Log("Setting screen: " + targetPosition + " " + targetContent + " content to " + newPosition);
        }
        else
        {
            Debug.Log("Setting screen: " + targetPosition + " " + targetContent + " content to " + newPosition);
        }
    }

    string GetContentsPosition(DCCWindow.contentID content)
    {
        string newPosition = "hidden";
        string[] quads = new string[] { "DCCquadScreen0",  "DCCquadScreen1", "DCCquadScreen2", "DCCquadScreen3", "DCCquadScreen4" };
        for (int i = 0; i < quads.Length; i++)
        {
            if (serverUtils.GetServerData(quads[i]) == (float)content)
            {
                newPosition = quads[i];
            }
        }

        return newPosition;
    }

    void SetServerScreenContent(string newPosition)
    {
        switch (selectedDirection)
        {
            case DCCScreenContentPositions.positionID.topLeft:
                RepositionContentAtDestination(DCCScreenContentPositions.positionID.topLeft, newPosition);
                serverUtils.PostServerData("DCCquadScreen0", (float)screenContent);
                break;
            case DCCScreenContentPositions.positionID.topRight:
                RepositionContentAtDestination(DCCScreenContentPositions.positionID.topRight, newPosition);
                serverUtils.PostServerData("DCCquadScreen1", (float)screenContent);
                break;
            case DCCScreenContentPositions.positionID.bottomLeft:
                RepositionContentAtDestination(DCCScreenContentPositions.positionID.bottomLeft, newPosition);
                serverUtils.PostServerData("DCCquadScreen2", (float)screenContent);
                break;
            case DCCScreenContentPositions.positionID.bottomRight:
                RepositionContentAtDestination(DCCScreenContentPositions.positionID.bottomRight, newPosition);
                serverUtils.PostServerData("DCCquadScreen3", (float)screenContent);
                break;
            case DCCScreenContentPositions.positionID.middle:
                RepositionContentAtDestination(DCCScreenContentPositions.positionID.middle, newPosition);
                serverUtils.PostServerData("DCCquadScreen4", (float)screenContent);
                break;
        }

        selectedDirection = DCCScreenContentPositions.positionID.hidden;
    }

	void Update ()
    {
        if (quadDirection.transform.localPosition.x < 0 && quadDirection.transform.localPosition.y > 0)
        {
            topLeftTime += Time.deltaTime;
            selectedDirection = DCCScreenContentPositions.positionID.topLeft;
        }
        else
        {
            topLeftTime = 0;
        }

        if (quadDirection.transform.localPosition.x > 0 && quadDirection.transform.localPosition.y > 0)
        {
            topRightTime += Time.deltaTime;
            selectedDirection = DCCScreenContentPositions.positionID.topRight;
        }
        else
        {
            topRightTime = 0;
        }

        if (quadDirection.transform.localPosition.x < 0 && quadDirection.transform.localPosition.y < 0)
        {
            bottomLeftTime += Time.deltaTime;
            selectedDirection = DCCScreenContentPositions.positionID.bottomLeft;
        }
        else
        {
            bottomLeftTime = 0;
        }

        if (quadDirection.transform.localPosition.x > 0 && quadDirection.transform.localPosition.y < 0)
        {
            bottomRightTime += Time.deltaTime;
            selectedDirection = DCCScreenContentPositions.positionID.bottomRight;
        }
        else
        {
            bottomRightTime = 0;
        }

        float topLeftLerp = hilightSpeed / topLeftTime;
        float topRightLerp = hilightSpeed / topRightTime;
        float bottomLeftLerp = hilightSpeed / bottomLeftTime;
        float bottomRightLerp = hilightSpeed / bottomRightTime;

        topLeftColor = Color.Lerp(unhilighted, highlighted, 1 - topLeftLerp);
        topRightColor = Color.Lerp(unhilighted, highlighted, 1 - topRightLerp);
        bottomLeftColor = Color.Lerp(unhilighted, highlighted, 1 - bottomLeftLerp);
        bottomRightColor = Color.Lerp(unhilighted, highlighted, 1 - bottomRightLerp);

        quadDirections[0].GetComponent<Renderer>().material.SetColor("_TintColor", topLeftColor);
        quadDirections[1].GetComponent<Renderer>().material.SetColor("_TintColor", topRightColor);
        quadDirections[2].GetComponent<Renderer>().material.SetColor("_TintColor", bottomLeftColor);
        quadDirections[3].GetComponent<Renderer>().material.SetColor("_TintColor", bottomRightColor);

    }
}
