using UnityEngine;
using System.Collections;
using Meg.DCC;

public class DCCWindow : MonoBehaviour
{
    public bool quadWindow;
    public DCCScreenContentPositions.positionID quadPosition;
    public enum contentID
    {
        none,
        instruments,
        thrusters,
        nav,
        sonar,
        diagnostics,
        lifesupport,
        piloting,
        comms,
        power,
        oxygen,
        batteries
    }

    public contentID windowContent;
    public bool hasFocus;
    public bool isLerping = false;
    public float lerpTime = 0.6f;
    private Vector3 toPosition;
    private Vector3 fromPosition;
    private Vector2 toScale;
    private Vector2 fromScale;
    private float lerpTimer = 0f;
    private graphicsDCCWindowSize window;

    public void MoveWindow(DCCScreenContentPositions.positionID destination)
    {
        quadPosition = destination;
        isLerping = true;
        lerpTimer = 0;
        toPosition = DCCScreenContentPositions.GetScreenPosition(destination);
        fromPosition = transform.localPosition;

        toScale = DCCScreenContentPositions.GetScreenScale(destination);
        fromScale = new Vector2(window.windowWidth, window.windowHeight);
    }

    public void SetWindowPosition(DCCScreenContentPositions.positionID destination)
    {
        quadPosition = destination;

        DCCScreenContentPositions.SetScreenPos(transform, destination);
        DCCScreenContentPositions.SetScreenScale(transform, destination);
    }

    void OnEnable ()
    {
        if (!window)
            window = GetComponent<graphicsDCCWindowSize>();

        if (quadWindow)
        {
            DCCScreenContentPositions.SetScreenPos(transform, quadPosition);
            DCCScreenContentPositions.SetScreenScale(transform, quadPosition);
        }
    }

    void Awake ()
    {
        if (!window)
            window = GetComponent<graphicsDCCWindowSize>();
    }

	void Update ()
    {
        if (isLerping)
        {
            lerpTimer += Time.deltaTime;

            float lerpAmount = lerpTimer / lerpTime;
            transform.localPosition = Vector3.Lerp(fromPosition, toPosition, lerpAmount);
            Vector2 windowScale = Vector2.Lerp(fromScale, toScale, lerpAmount);
            window.windowWidth = windowScale.x;
            window.windowHeight = windowScale.y;

            if (lerpTimer > lerpTime)
            {
                isLerping = false;
            }
            if (quadPosition == DCCScreenContentPositions.positionID.hidden)
            {
                //gameObject.SetActive(false);
            }
        }

        if (!quadWindow)
        {
            if (hasFocus)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
            else
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 10f);
        }
	}
}
