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
    private Vector2 toPosition;
    private Vector2 fromPosition;
    private Vector2 toScale;
    private Vector2 fromScale;
    private float lerpTimer = 0f;
    private graphicsDCCWindowSize window;

    public GameObject transitionBack;
    public Color transitionFromColor = Color.black;
    public Color transitionToColor = Color.black;

    private float transitionTimer;
    private bool isTransitioning;
    private Renderer r1;

    public void MoveWindow(DCCScreenContentPositions.positionID destination)
    {
        quadPosition = destination;
        isLerping = true;
        lerpTimer = 0;
        toPosition = DCCScreenContentPositions.GetScreenPosition(destination);
        fromPosition = transform.localPosition;

        toScale = DCCScreenContentPositions.GetScreenScale(destination);
        fromScale = new Vector2(window.windowWidth, window.windowHeight);

        r1.material.SetColor("_TintColor", transitionToColor);

        
    }

    public void SetWindowPosition(DCCScreenContentPositions.positionID destination)
    {
        quadPosition = destination;
        isTransitioning = true;
        transitionTimer = 0;

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

        r1 = transitionBack.GetComponent<Renderer>();
    }

	void Update ()
    {
        if (isLerping)
        {
            lerpTimer += Time.deltaTime;
            transitionBack.SetActive(true);

            float lerpAmount = lerpTimer / lerpTime;
            transform.localPosition = Vector2.Lerp(fromPosition, toPosition, lerpAmount);
            Vector2 windowScale = Vector2.Lerp(fromScale, toScale, lerpAmount);
            window.windowWidth = windowScale.x;
            window.windowHeight = windowScale.y;

            //color fade
            Color lerpColor = Color.Lerp(transitionToColor, transitionFromColor, lerpAmount);
            r1.material.SetColor("_TintColor", lerpColor);

            if (lerpTimer > lerpTime)
            {
                isLerping = false;
                isTransitioning = true;
                transitionTimer = 0;
            }
        }
        else if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            transitionBack.SetActive(true);

            float lerpAmount = transitionTimer / 0.3f;

            //color fade
            Color lerpColor = Color.Lerp(transitionFromColor, transitionToColor, lerpAmount);
            r1.material.SetColor("_TintColor", lerpColor);

            if (transitionTimer > 0.3f)
            {
                isTransitioning = false;
                transitionTimer = 0;
            }

        }
        else
        {
            transitionBack.SetActive(false);
        }

        if (hasFocus)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -10f);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
        }
	}
}
