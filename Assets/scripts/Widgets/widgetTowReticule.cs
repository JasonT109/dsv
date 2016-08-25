using UnityEngine;
using System.Collections;
using Meg.Networking;

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

    private Vector2 moveDir = Vector2.zero;
    // private Vector2 newMoveDir = Vector2.zero;
    private float distanceLerp;
    private float distance;
    private Vector3 initialPos;
    // private Vector3 startPos;
    private bool locked;
    private float lockedTime;


	// Use this for initialization
	void Start ()
    {
        initialPos = transform.position;
        // startPos = transform.position;
    }
	
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

    // Update is called once per frame
    void Update ()
    {
        distance = Vector3.Distance(initialPos, target.position);
        distanceLerp = (Mathf.Abs(distance - lockDistance)) / (trackDistance - lockDistance);

        if (distance < lockDistance)
        {
            transform.position = target.position;
            locked = true;
            target.GetComponent<Renderer>().material.SetColor("_TintColor", lockColor);
            lockRing1.GetComponent<Renderer>().material.SetColor("_TintColor", lockColor);
            lockRing1.transform.localScale = lockRing1EndScale;
            targetLockText.SetActive(true);
            targetText.Text = "LOCKED";
            targetText.Color = textLockedColor;
        }
        else if (distance > lockDistance && distance < trackDistance)
        {
            transform.position = Vector3.Lerp(initialPos, target.position, 1 - distanceLerp);
            target.GetComponent<Renderer>().material.SetColor("_TintColor", Color.Lerp(targetColor, lockColor, 1 - distanceLerp));
            lockRing1.GetComponent<Renderer>().material.SetColor("_TintColor", lockColor);
            lockRing1.transform.localScale = Vector3.Lerp(lockRing1StartScale, lockRing1EndScale, 1 - distanceLerp);
            locked = false;
            lockedTime = 0;
            targetLockText.SetActive(false);
            targetText.Text = "AQUIRING...";
            targetText.Color = textUnlockedColor;
        }
        else
        {
            transform.position = initialPos;
            target.GetComponent<Renderer>().material.SetColor("_TintColor", targetColor);
            lockRing1.GetComponent<Renderer>().material.SetColor("_TintColor", targetColor);
            lockRing1.transform.localScale = lockRing1StartScale;
            locked = false;
            lockedTime = 0;
            targetLockText.SetActive(false);
            targetText.Text = "NO TARGET";
            targetText.Color = textUnlockedColor;
        }

        if (locked)
        {
            lockedTime += Time.deltaTime;
        }

        for (int i = 0; i < lockedOnGFX.Length; i++)
        {
            if (locked && lockedTime > (0.2f * (i+1)))
            {
                lockedOnGFX[i].SetActive(true);
            }
            else
            {
                lockedOnGFX[i].SetActive(false);
            }
        }

        moveDir.y = -serverUtils.GetServerData("inputYaxis");
        moveDir.x = serverUtils.GetServerData("inputXaxis");

        moveDir *= moveSpeed;

        initialPos = new Vector3(initialPos.x + moveDir.x, initialPos.y + moveDir.y, initialPos.z);

        //set position of indicator
        initPosIndicator.position = initialPos;
        initPosIndicator.localPosition = new Vector3(Mathf.Clamp(initPosIndicator.localPosition.x, -maxX, maxX), Mathf.Clamp(initPosIndicator.localPosition.y, -maxY, maxY), initPosIndicator.localPosition.z);
        initialPos = initPosIndicator.position;


        gameObject.transform.Translate(moveDir);
    }
}
