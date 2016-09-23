using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Meg.Networking;

public class SubText : MonoBehaviour
{
    //public Text CanText;
    //public DynamicText DynText;

    public string DataName;
    public string prefix;
    public string suffix;

    public string Decimals = "0";

	// Use this for initialization
	void Start ()
    {
        if (this.GetComponent<Text>())
        {
            this.GetComponent<Text>().text = prefix + serverUtils.GetServerDataRaw(DataName).ToString("n" + Decimals) + suffix;
        }

        if (this.GetComponent<DynamicText>())
        {
            this.GetComponent<DynamicText>().SetText(prefix + serverUtils.GetServerDataRaw(DataName).ToString("n" + Decimals) + suffix);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (this.GetComponent<Text>())
        {
            this.GetComponent<Text>().text = prefix + serverUtils.GetServerDataRaw(DataName).ToString("n" + Decimals) + suffix;
        }

        if (this.GetComponent<DynamicText>())
        {
            this.GetComponent<DynamicText>().SetText(prefix + serverUtils.GetServerDataRaw(DataName).ToString("n" + Decimals) + suffix);
        }
    }
}
