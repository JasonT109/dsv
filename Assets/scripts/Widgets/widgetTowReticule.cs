using UnityEngine;
using System.Collections;
using Meg.Networking;
using Meg.Maths;
using Meg.Graphics;

public class widgetTowReticule : MonoBehaviour
{

    [Header("Required Components")]
    public Transform target;
    public GameObject[] lockedOnGFX;
    public GameObject lockRing1;
    public GameObject targetLockText;
    public widgetText targetText;
    public Transform initPosIndicator;

    [Header("Target Configuration")]
    public Color targetColor;
    public Color lockColor;
    public Vector3 lockRing1StartScale = new Vector3(1, 1, 1);
    public Vector3 lockRing1EndScale = new Vector3(1, 1, 1);
    public float lockDistance = 1f;
    public float trackDistance = 2f;

    [Header("Reticule Configuration")]
    public float maxX = 6;
    public float maxY = 5;
    public float moveSpeed = 1;

    [Header("Text Configuration")]
    public Color textUnlockedColor;
    public Color textLockedColor;
    public Color textAcquiringColor;
    public Color textFiredColor;

    [Header("Text Objects")]
    public widgetText lineLengthText;


    [Header("Text Flashing Configuration")]
    public float flashSpeed = 1;
    public float flashPause = -1;

    private float flashTimer = 0;
    private Vector2 moveDir = Vector2.zero;
    private float distanceLerp;
    private float distance;
    private Vector3 initialPos;
    private bool locked;
    private float lockedTime;
    private bool fired = false;
    private float torpedoTimer = 0;
    private float lineLengthValue;
    private float updateTimer = 0;
    private float torpedoTravelTime = 0;

    Vector3 getLocalPosition(Vector3 position, Transform localSpace)
    {
        Vector3 localPosition = localSpace.InverseTransformPoint(position);

        return localPosition;
    }

    Vector3 getWorldPosition(Vector3 position, Transform worldSpace)
    {
        Vector3 worldPosition = worldSpace.TransformPoint(position);

        return worldPosition;
    }

    /** Sets the target status text. */
    void SetTargetText()
    {

        int status = (int)serverUtils.GetServerData("towFiringStatus");

        switch (status)
        {
            case 0:
                targetText.Text = "NO TARGET";
                break;
            case 1:
                targetText.Text = "AQUIRING...";
                break;
            case 2:
                targetText.Text = "LOCKED";
                break;
            case 3:
                targetText.Text = "LAUNCHED";
                break;
            default:
                targetText.Text = "NO TARGET";
                break;
        }
    }

    /** Set the reticules position, status and colors. */
    void SetTarget(Vector3 position, bool status, Vector3 ringScale, Color targetColor, Color ringColor, Color textColor)
    {
        //position
        transform.position = position;

        //status
        locked = status;
        targetLockText.SetActive(status);

        //scale
        lockRing1.transform.localScale = ringScale;

        //colors
        target.GetComponent<Renderer>().material.SetColor("_TintColor", targetColor);
        lockRing1.GetComponent<Renderer>().material.SetColor("_TintColor", ringColor);

        if (serverUtils.GetServerData("towFiringStatus") != 3)
            targetText.Color = textColor;
        //else
            //targetText.Color = textFiredColor;
    }

    /** Set the targets color. */
    void SetTargetColor()
    {
        if (serverUtils.GetServerData("towtargetvisible") == 0)
        {
            if (target.GetComponent<Renderer>().material.GetColor("_TintColor").a > 0)
                targetColor = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a - Time.deltaTime);
        }

        if (serverUtils.GetServerData("towtargetvisible") > 0)
        {
            if (target.GetComponent<Renderer>().material.GetColor("_TintColor").a < 1)
                targetColor = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a + Time.deltaTime);
        }
    }

    /** Set the visibility of locked on gfx. */
    void SetLockedVis()
    {
        for (int i = 0; i < lockedOnGFX.Length; i++)
        {
            if (locked && lockedTime > (0.2f * (i + 1)))
            {
                lockedOnGFX[i].SetActive(true);
            }
            else
            {
                lockedOnGFX[i].SetActive(false);
            }
        }
    }

    /** Handles the torpedo / grapple launch graphics. */
    void FireTorpedo()
    {
        if (!fired)
        {
            fired = true;
            torpedoTravelTime = 0;
        }

        torpedoTravelTime += Time.deltaTime;

        float lineRemaining = serverUtils.GetServerData("towlineremaining");
        float decay = 1;

        if (torpedoTravelTime > 5)
            decay = graphicsEasing.EaseIn(graphicsMaths.remapValue(torpedoTravelTime, 5, 45, 1, 0), EasingType.Quadratic);

        float towLineSpeed = (serverUtils.GetServerData("towfiringpressure") * decay) * 0.1f;
        float lineLength = serverUtils.GetServerData("towlinelength");
        float lineDistance = Mathf.Clamp(serverUtils.GetServerData("towlineremaining") - (Time.deltaTime * towLineSpeed), 0, 1000);
        lineLengthValue = (lineLength - lineDistance);

        torpedoTimer += Time.deltaTime;

        if (torpedoTimer < 0.1f)
            return;

        torpedoTimer = 0f;

        if (lineRemaining > 0)
        {
            serverUtils.PostServerData("towlinespeed", towLineSpeed * 0.1f);
            serverUtils.PostServerData("towlineremaining", lineDistance);
        }
        else
        {
            serverUtils.PostServerData("towlinespeed", 0);
        }
    }

    void ResetTorpedo()
    {
        lineLengthValue = 0;
        serverUtils.PostServerData("towlineremaining", serverUtils.GetServerData("towlinelength"));
        serverUtils.PostServerData("towlinespeed", 0);
        torpedoTravelTime = 0;
        fired = false;
    }

    void UpdateText()
    {
        updateTimer += Time.deltaTime;

        if (updateTimer < 0.2f)
            return;

        updateTimer = 0;

        lineLengthText.Text = lineLengthValue.ToString("n0") + "m";
    }

    void Start()
    {
        initialPos = transform.position;
    }

    void Update ()
    {

        UpdateText();

        distance = Vector3.Distance(initialPos, target.position);
        distanceLerp = (Mathf.Abs(distance - lockDistance)) / (trackDistance - lockDistance);

        if (distance < lockDistance && serverUtils.GetServerData("towtargetvisible") == 1)
        {
            SetTarget(target.position, true, lockRing1EndScale, lockColor, lockColor, textLockedColor);

            if (serverUtils.GetServerData("towFiringStatus") < 2)
                serverUtils.PostServerData("towfiringstatus", 2);
        }
        else if (distance > lockDistance && distance < trackDistance && serverUtils.GetServerData("towtargetvisible") == 1)
        {
            SetTarget(Vector3.Lerp(initialPos, target.position, 1 - distanceLerp), false, Vector3.Lerp(lockRing1StartScale, lockRing1EndScale, 1 - distanceLerp), Color.Lerp(targetColor, lockColor, 1 - distanceLerp), lockColor, textAcquiringColor);
            lockedTime = 0;

            if (serverUtils.GetServerData("towFiringStatus") != 3 && serverUtils.GetServerData("towFiringStatus") != 1)
                serverUtils.PostServerData("towfiringstatus", 1);
        }
        else
        {
            SetTarget(initialPos, false, lockRing1StartScale, targetColor, targetColor, textUnlockedColor);
            lockedTime = 0;

            if (serverUtils.GetServerData("towFiringStatus") != 3 && serverUtils.GetServerData("towFiringStatus") != 0)
                serverUtils.PostServerData("towfiringstatus", 0);
        }

        SetTargetText();

        SetTargetColor();

        if (locked)
        {
            lockedTime += Time.deltaTime;
        }

        SetLockedVis();

        if (serverUtils.GetServerData("towFiringStatus") == 3)
        {
            FireTorpedo();

            flashTimer += Time.deltaTime * flashSpeed;
            float sinWave = Mathf.Sin(flashTimer);

            if (flashTimer > flashSpeed)
                flashTimer = flashPause;

            targetText.Color = Color.Lerp(Color.black, textFiredColor, Mathf.Sin(sinWave));
        }

        if (serverUtils.GetServerData("towFiringStatus") != 3 && fired)
            ResetTorpedo();

        moveDir.y = -serverUtils.GetServerData("inputYaxis");
        moveDir.x = serverUtils.GetServerData("inputXaxis");
        moveDir *= moveSpeed;
        initialPos = new Vector3(initialPos.x + moveDir.x, initialPos.y + moveDir.y, initialPos.z);

        initPosIndicator.position = initialPos;
        initPosIndicator.localPosition = new Vector3(Mathf.Clamp(initPosIndicator.localPosition.x, -maxX, maxX), Mathf.Clamp(initPosIndicator.localPosition.y, -maxY, maxY), initPosIndicator.localPosition.z);
        initialPos = initPosIndicator.position;

        gameObject.transform.Translate(moveDir);
    }
}
