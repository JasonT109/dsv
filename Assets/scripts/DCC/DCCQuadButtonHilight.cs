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

    [Header("Parent Button Control")]
    public buttonControl parentButton;

    private Vector3 transformDirection;
    private Color topLeftColor = Color.gray;
    private Color topRightColor = Color.gray;
    private Color bottomLeftColor = Color.gray;
    private Color bottomRightColor = Color.gray;

    private float topLeftTime = 0f;
    private float topRightTime = 0f;
    private float bottomLeftTime = 0f;
    private float bottomRightTime = 0f;

    private bool waiting = false;
    private GameObject colorThemeObject;

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
            string newPosition = GetContentsPosition(screenContent);
            SetServerScreenContent(newPosition);
        }

        transformDirection = Vector3.zero;
        quadDirection.transform.localPosition = Vector3.zero;
    }

    /** Swaps position of content at destination quad box. */
    void RepositionContentAtDestination(DCCScreenContentPositions.positionID targetPosition, string newPosition)
    {
        var newPositionId = DCCScreenData.GetPositionForName(newPosition);
        var targetContent = serverUtils.GetQuadContent(targetPosition, DCCScreenData.StationId);
        if (newPositionId != DCCScreenContentPositions.positionID.hidden)
            serverUtils.PostQuadContent(newPositionId, targetContent, DCCScreenData.StationId);
    }

    /** Returns the quad that this content inhabits. */
    static string GetContentsPosition(DCCWindow.contentID content)
    {

        var newPosition = "hidden";
        string[] quads = { "DCCquadScreen0", "DCCquadScreen1", "DCCquadScreen2", "DCCquadScreen3", "DCCquadScreen4" };
        foreach (var quad in quads)
        {
            var id = DCCScreenData.GetPositionForName(quad);
            if (serverUtils.GetQuadContent(id, DCCScreenData.StationId) == content)
                newPosition = quad;
        }

        return newPosition;
    }

    /** Sets the screen content. A string new position is required for repositioning old content. */
    void SetServerScreenContent(string newPosition)
    {
        RepositionContentAtDestination(selectedDirection, newPosition);
        serverUtils.PostQuadContent(selectedDirection, screenContent, DCCScreenData.StationId);
        selectedDirection = DCCScreenContentPositions.positionID.hidden;
    }

	void Update ()
    {
        if (parentButton.DCCQuadButton)
        {
            if (colorThemeObject == null)
                colorThemeObject = GameObject.FindWithTag("ServerData");

            if (colorThemeObject)
            {
                highlighted = colorThemeObject.GetComponent<graphicsColourHolder>().keyColor;
                var hsb = HSBColor.FromColor(highlighted);
                hsb.b = Mathf.Max(0, hsb.b * 0.2f);
                unhilighted = hsb.ToColor();
            }

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

            if (parentButton.doublePressed && !waiting)
            {
                waiting = true;
                StartCoroutine(pressWait(0.1f));

                //if we are not switching to new content
                var quadMiddle = serverUtils.GetQuadContent(DCCScreenContentPositions.positionID.middle, DCCScreenData.StationId);
                if (screenContent == quadMiddle)
                {
                    if (serverUtils.GetQuadFullScreen(DCCScreenData.StationId) == 1)
                    {
                        selectedDirection = DCCScreenContentPositions.positionID.hidden;
                        serverUtils.PostQuadFullScreen(0, DCCScreenData.StationId);
                        serverUtils.PostQuadContent(DCCScreenContentPositions.positionID.middle, 0, DCCScreenData.StationId);
                    }
                    else
                    {
                        selectedDirection = DCCScreenContentPositions.positionID.middle;
                        serverUtils.PostQuadFullScreen(1, DCCScreenData.StationId);
                    }
                }
                else
                {
                    //put our new content into this box and make sure middle screen is shown
                    selectedDirection = DCCScreenContentPositions.positionID.middle;
                    serverUtils.PostQuadFullScreen(1, DCCScreenData.StationId);
                }
            }
        }
    }

    IEnumerator pressWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        waiting = false;
    }
}
