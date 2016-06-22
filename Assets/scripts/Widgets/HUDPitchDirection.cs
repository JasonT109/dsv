using UnityEngine;
using System.Collections;
using Meg.Networking;

public class HUDPitchDirection : MonoBehaviour {

    //public GameObject inputs;
    public bool yaw = false;
    public bool pitch = false;
    public bool thrustVectorL = false;
    public bool thrustVectorR = false;
    public GameObject thrusterControl;
    public float visibleRange = 90;
    public float visibleTicks;
    public GameObject mainTick;
    public GameObject subTick;
    public GameObject subTick2;
    public bool useAltSubTicks = false;
    public bool yFlipSubTicks = false;
    public Vector3 textPosition = new Vector3(0,0,0);
    public Vector3 textRotation = new Vector3(0,0,0);
    public float endTolerance = 0.02f;
    public string[] tickNames = new string[8] { "N","NE","E","SE","S","SW","W","NW" };
    public float startPoint;
    public float endPoint;
    public int numSubTicks;
    public float labelShift = 0;

    private GameObject serverObject;
    private GameObject[] mainTicks;
    private float distance;
    private float lShift = 0;
    private float fullRangeDistance = 0;
    private float degreeDistance = 0;
    private float heading;
    private float prevHeading = 0;

    // Use this for initialization
    void Start ()
    {
        //how many ticks will be visible
        visibleTicks = 8 / (360 / visibleRange);

        //shift the labels by amount
        lShift = labelShift;

        //set up our main ticks array
        mainTicks = new GameObject[(int)((visibleTicks) + ((visibleTicks) * numSubTicks))];

        //initialX = new float[mainTicks.Length];

        //get distance between each main tick
        distance = (endPoint - startPoint) / visibleTicks;

        //get full distance
        fullRangeDistance = (endPoint - startPoint) * (360 / visibleRange);

        //get distance that represents 1 degree
        degreeDistance = fullRangeDistance / 360;

        //set up position of first tick
        float pos = startPoint;
        float spos = 0;
        int t = 0;
        TextMesh label;
        int altSubTick = (int) Mathf.Floor(numSubTicks * 0.5f);
        
        //instantiate main ticks
        for (int i = 0; i < visibleTicks; i++)
        {
            GameObject mTick = Instantiate(mainTick, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            mTick.transform.parent = gameObject.transform;
            mTick.transform.localPosition = new Vector3(pos, 0, 0);
            if (pitch)
            {
                mTick.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            TextMesh txt = mTick.GetComponentInChildren<TextMesh>();
            if (txt)
            {
                txt.transform.localPosition = textPosition;
                txt.transform.rotation = Quaternion.Euler(textRotation);
            }
            spos = pos;
            mainTicks[t] = mTick;
            label = mTick.GetComponentInChildren<TextMesh>();
            label.text = tickNames[(int)lShift];
            lShift++;

                if (lShift > (tickNames.Length - 1))
            {
                lShift = 0;
            }
            if (i != visibleTicks)
            {
                for (int s = 0; s < numSubTicks; s++)
                {
                    spos += distance / (numSubTicks + 1);

                    GameObject newTick = subTick;

                    if (useAltSubTicks)
                    {
                        if (s == altSubTick)
                        {
                            //Debug.Log("Creating alt sub tick");
                            newTick = subTick2;
                        }
                    }
                    GameObject sTick = Instantiate(newTick, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;

                    sTick.transform.parent = gameObject.transform;
                    sTick.transform.localPosition = new Vector3(spos, 0, 0);
                    sTick.transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (pitch)
                    {
                        sTick.transform.rotation = Quaternion.Euler(0, 0, -90);
                    }
                    if (yFlipSubTicks)
                    {
                        sTick.transform.localScale = new Vector3(sTick.transform.localScale.x, -sTick.transform.localScale.y, sTick.transform.localScale.z);
                    }

                    t++;
                    mainTicks[t] = sTick;
                }
            }
            pos += distance;
            t++;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (thrustVectorL && thrusterControl)
        {
            heading = Mathf.Lerp(heading, thrusterControl.GetComponent<widgetThrusterControl>().thrusterVectorAngleL * 0.0085f, Time.deltaTime * 2.0f);
        }
        if (thrustVectorR && thrusterControl)
        {
            heading = Mathf.Lerp(heading, thrusterControl.GetComponent<widgetThrusterControl>().thrusterVectorAngleR * 0.0085f, Time.deltaTime * 2.0f);
        }

        if (yaw)
        {
            heading = serverUtils.GetServerData("heading"); 
        }
        if (pitch)
        {
            heading = serverUtils.GetServerData("pitchAngle");
        }
        for (int i = 0; i < mainTicks.Length; i++) //for each major point on the compass
        {
            //cycling around can cause a visual glitch
            if (prevHeading > 270 && heading < 90)
            {
                prevHeading = heading;
            }
            if (prevHeading < 90 && heading > 270)
            {
                prevHeading = heading;
            }
            mainTicks[i].transform.Translate(((prevHeading - heading) * degreeDistance) * 10, 0, 0, Space.Self);
            TextMesh t = mainTicks[i].GetComponentInChildren<TextMesh>();
            if (mainTicks[i].transform.localPosition.x < (startPoint - endTolerance))
            {
                mainTicks[i].transform.localPosition = new Vector3(mainTicks[i].transform.localPosition.x + (endPoint - startPoint), 0, 0);
                
                if (t)
                {
                    int index = System.Array.IndexOf(tickNames, t.text);
                    int newIndex = index - 4;
                    if (newIndex < 0)
                    {
                        newIndex += tickNames.Length;
                    }
                    t.text = tickNames[newIndex];
                }
            }
            if (mainTicks[i].transform.localPosition.x > (endPoint + endTolerance))
            {
                mainTicks[i].transform.localPosition = new Vector3(mainTicks[i].transform.localPosition.x - (endPoint - startPoint), 0, 0);
                if (t)
                {
                    int index = System.Array.IndexOf(tickNames, t.text);
                    int newIndex = index + 4;
                    if (newIndex > (tickNames.Length - 1))
                    {
                        newIndex -= tickNames.Length;
                    }
                    t.text = tickNames[newIndex];
                }
            }
            if (t)
            {
                //fade to the text as it approaches the edge
                float diff = startPoint * 0.5f;
                
                if (mainTicks[i].transform.localPosition.x < diff)
                {
                    float pos = mainTicks[i].transform.localPosition.x - diff;
                    float lerpValue = pos / diff;
                    Color newColor = new Color(1f, 1f, 1f, Mathf.Lerp(0.5f, 0, lerpValue));
                    t.GetComponent<Renderer>().material.color = newColor;
                }
                else if (mainTicks[i].transform.localPosition.x > -diff)
                {   
                    float pos = mainTicks[i].transform.localPosition.x + diff;
                    float lerpValue = 1 - (pos / -diff);
                    Color newColor = new Color(1f, 1f, 1f, Mathf.Lerp(0, 0.5f, lerpValue));
                    t.GetComponent<Renderer>().material.color = newColor;
                }
                else
                {
                    Color newColor = new Color(1f, 1f, 1f, 0.5f);
                    t.GetComponent<Renderer>().material.color = newColor;
                }
            }
        }
        prevHeading = heading;
    }
}
