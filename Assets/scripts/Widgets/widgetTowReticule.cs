using UnityEngine;
using System.Collections;

public class widgetTowReticule : MonoBehaviour
{

    public Transform target;
    public float lockDistance = 1f;
    public float trackDistance = 2f;
    public GameObject[] lockedOnGFX;
    public Color targetColor;
    public Color lockColor;
    public GameObject lockRing1;
    public Vector3 lockRing1StartScale = new Vector3(1,1,1);
    public Vector3 lockRing1EndScale = new Vector3(1, 1, 1);
    public GameObject targetLockText;
    public widgetText targetText;

    private float distanceLerp;
    private float distance;
    private Vector3 initialPos;
    private bool locked;
    private float lockedTime;


	// Use this for initialization
	void Start ()
    {
        initialPos = transform.position;
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
    }
}
