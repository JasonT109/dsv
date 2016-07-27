using UnityEngine;
using System.Collections;
using Meg.Networking;

public class digital_gauge : MonoBehaviour {

    private GameObject tacho;
    public bool useThrottle = false;
    public bool useNegThrottle = false;
    public bool useX_pos = false;
    public bool useX_neg = false;
    public bool useY_pos = false;
    public bool useY_neg = false;
    public bool useX2_pos = false;
    public bool useX2_neg = false;
    public bool useY2_pos = false;
    public bool useY2_neg = false;
    public bool useSpeed = false;
    public bool useStaticValue = false;
    public int staticValue = 0;
    public bool useValueFromServer = false;
    public string linkDataString = "depth";
    public GameObject serverObject;
    public bool useDigitalNeedle = false;
    public GameObject digitalNeedle;
    public GameObject minTick;
    public GameObject maxTick;
    public float smooth = 2.0f;
    public float startAngle = 0.0f;
    public float endAngle = 0.0f;
    public float currentAngle = 0.0f;
    public float inputValue = 0.0f;
    public int value = 0;
    public int maxValue = 100;
    public int minValue = 0;
    public bool doWobble = false;
    public float valueWobble = 0.0f;
    public float wobbleSpeed = 0.0f;
    public float needleNoise = 1.0f;
    private float wobble = 1.0f;
    private float wobbleAccumulate = 0.0f;
    public GameObject[] digitalTicks;
    public GameObject optionalText;
    public Color[] colorGradient;
    public bool mapGradientToTicks = false;
    private tachometer tachoScript;

    private widgetText text;
    private TextMesh textM;
    private DynamicText textD;

    private float numTicks = 10;
    private float tickRange = 1.0f;
    private float index;

    // Use this for initialization
    void Start ()
    {
        serverObject = null;
        numTicks = digitalTicks.Length;
        if(optionalText)
        {
            text = optionalText.GetComponent<widgetText>();
            textM = optionalText.GetComponent<TextMesh>();
            textD = optionalText.GetComponent<DynamicText>();
        }
        if (serverObject == null)
        {
            serverObject = GameObject.FindWithTag("ServerData");
        }
        else
        {
            if (useThrottle)
            {
                value = (int)serverUtils.GetServerData("inputZaxis");
            }
        }
        if (useDigitalNeedle)
        {
            value = staticValue;
            currentAngle = ((float)value - minValue) / (maxValue - minValue) * (endAngle - startAngle) + startAngle;
            Quaternion qAngle = Quaternion.Euler(0, 0, currentAngle);
            digitalNeedle.transform.rotation = qAngle;
            if (minTick)
                minTick.transform.rotation = Quaternion.Euler(0, 0, startAngle);
            if (maxTick)
                maxTick.transform.rotation = Quaternion.Euler(0, 0, endAngle);
        }
        else
        {
            if (digitalNeedle)
            {
                digitalNeedle.SetActive(false);
                if (minTick)
                    minTick.SetActive(false);
                if (maxTick)
                    maxTick.SetActive(false);
            }
        }

        

        for (int i = 0; i < numTicks; i++)
            digitalTicks[i].SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (serverObject == null)
        {
            serverObject = GameObject.FindWithTag("ServerData");
        }
        else
        {
            
            //= (X-A)/(B-A) * (D-C) + C
            if (useThrottle)
            {
                if (serverUtils.GetServerData("inputZaxis") >= 0.0f)
                {
                    inputValue = serverUtils.GetServerData("inputZaxis") * maxValue;
                    value = Mathf.RoundToInt(inputValue);
                }
                else
                {
                    value = 0;
                }
            }

            if (useNegThrottle)
            {
                if (serverUtils.GetServerData("inputZaxis") <= 0.0f)
                {
                    inputValue = Mathf.Abs(serverUtils.GetServerData("inputZaxis") * maxValue);
                    value = Mathf.RoundToInt(inputValue);
                }
                else
                {
                    value = 0;
                }
            }

            if (useX_pos)
            {
                if (serverUtils.GetServerData("inputXaxis") >= 0.0f)
                {
                    inputValue = serverUtils.GetServerData("inputXaxis") * maxValue;
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useX_neg)
            {
                if (serverUtils.GetServerData("inputXaxis") <= 0.0f)
                {
                    inputValue = Mathf.Abs(serverUtils.GetServerData("inputXaxis") * maxValue);
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useY_pos)
            {
                if (serverUtils.GetServerData("inputYaxis") >= 0.0f)
                {
                    inputValue = serverUtils.GetServerData("inputYaxis") * maxValue;
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useY_neg)
            {
                if (serverUtils.GetServerData("inputYaxis") <= 0.0f)
                {
                    inputValue = Mathf.Abs(serverUtils.GetServerData("inputYaxis") * maxValue);
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useX2_pos)
            {
                if (serverUtils.GetServerData("inputXaxis2") >= 0.0f)
                {
                    inputValue = serverUtils.GetServerData("inputXaxis2") * maxValue;
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useX2_neg)
            {
                if (serverUtils.GetServerData("inputXaxis2") <= 0.0f)
                {
                    inputValue = Mathf.Abs(serverUtils.GetServerData("inputXaxis2") * maxValue);
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useY2_pos)
            {
                if (serverUtils.GetServerData("inputYaxis2") >= 0.0f)
                {
                    inputValue = serverUtils.GetServerData("inputYaxis2") * maxValue;
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useY2_neg)
            {
                if (serverUtils.GetServerData("inputYaxis2") <= 0.0f)
                {
                    inputValue = Mathf.Abs(serverUtils.GetServerData("inputYaxis2") * maxValue);
                    value = (int)inputValue;
                }
                else
                {
                    value = 0;
                }
            }

            if (useSpeed)
            {
                value = (int)serverUtils.GetServerData("velocity");
            }

            if (useStaticValue)
            {
                value = staticValue;
            }

            if (doWobble)
            {
                index += Time.deltaTime;
                wobble = valueWobble * Mathf.Sin(wobbleSpeed * index);
                wobbleAccumulate += wobble * 0.1f;
                value += Mathf.RoundToInt(wobbleAccumulate);
                value = Mathf.Clamp(value, minValue, maxValue);
            }

            if (useDigitalNeedle)
            {
                currentAngle = ((float)value - minValue) / (maxValue - minValue) * (endAngle - startAngle) + startAngle;
                Quaternion qAngle = Quaternion.Euler(0, 0, (currentAngle + Random.Range(-needleNoise, needleNoise)));
                digitalNeedle.transform.rotation = Quaternion.Slerp(digitalNeedle.transform.rotation, qAngle, Time.deltaTime * smooth);
            }
            else
            {
                tickRange = (maxValue - minValue) / (numTicks - 1);
                for (int i = 0; i < numTicks; i++)
                {
                    if ((float)value >= (float)(i * tickRange) && value != 0)
                    {
                        digitalTicks[i].SetActive(true);
                    }
                    else
                    {
                        digitalTicks[i].SetActive(false);
                    }
                }
            }

            if (optionalText)
                Text = value.ToString("D3");

            if (useValueFromServer && serverObject)
            {
                value = (int)serverUtils.GetServerData(linkDataString);
            }
        }
    }

    private string Text
    {
        set
        {
            if (text)
                text.Text = value;
            else if (textM)
                textM.text = value;
            else if (textD)
                textD.SetText(value);
        }
    }
}
