using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Meg.Networking;

public class DeployLights : MonoBehaviour 
{
	private GameObject colourThemeObj;
	private Color kColor;

    public int lightID = 0;
	public bool light1 = false;
	public bool light2 = false;
	private int oldStatus = 10; //forces a state pass
	private int deployStatus = 0;
	// 0 = default off state
	// 1 = ready to launch
	// 2 = light 1 launched
    // 3 = both lights launched

	public Image ReadyLeft;
	public Image ReadyRight;
	public Image ReadyHighlight;

	public Image LeftPaddle;
	public Image RightPaddle;

	public DynamicText Title;
	public DynamicText LeftText;
	public DynamicText RightText;

	// Use this for initialization
	void Start () 
	{
		ReadyLeft.gameObject.SetActive(true);
		ReadyRight.gameObject.SetActive(true);
		ReadyHighlight.gameObject.SetActive(true);

		LeftPaddle.gameObject.SetActive(true);
		RightPaddle.gameObject.SetActive(true);

		DefaultState();
	}

	void Update () 
	{
        //get the server status of this object
        deployStatus = GetData(lightID);

        if (deployStatus == oldStatus)
		{
			//nothing has changed, do nothing.
			return;
		}

		oldStatus = deployStatus;

		switch(deployStatus)
		{
		case 0:
			{
				DefaultState();
			}
			return;
		case 1:
			{
				ReadyState();
			}
			return;
        case 2:
            {
                ReadyState_2();
            }
            return;
        case 3:
		    {
			    DeployedState();
		    }
		    return;
		}
	}

    /** Gets the color theme from the server object. */
    void ObtainThemeColour()
    {
        if (colourThemeObj == null)
            colourThemeObj = GameObject.FindWithTag("ServerData");

        if (colourThemeObj)
        {
            kColor = colourThemeObj.GetComponent<graphicsColourHolder>().theme.keyColor;
        }
    }

    private int GetData(int ID)
    {
        int value = 0;

        switch(ID)
        {
            case 1:
                value = (int)serverUtils.GetServerData("lightarray1");
                break;
            case 2:
                value = (int)serverUtils.GetServerData("lightarray2");
                break;
            case 3:
                value = (int)serverUtils.GetServerData("lightarray3");
                break;
            case 4:
                value = (int)serverUtils.GetServerData("lightarray4");
                break;
            case 5:
                value = (int)serverUtils.GetServerData("lightarray5");
                break;
            case 6:
                value = (int)serverUtils.GetServerData("lightarray6");
                break;
            case 7:
                value = (int)serverUtils.GetServerData("lightarray7");
                break;
            case 8:
                value = (int)serverUtils.GetServerData("lightarray8");
                break;
            case 9:
                value = (int)serverUtils.GetServerData("lightarray9");
                break;
            case 10:
                value = (int)serverUtils.GetServerData("lightarray10");
                break;
        }

        return value;
    }

    /** Post server data so everyone stays in sync. */
    private void PostData(int ID, float value)
    {
        switch(ID)
        {
            case 1:
                serverUtils.PostServerData("lightarray1", value);
                break;
            case 2:
                serverUtils.PostServerData("lightarray2", value);
                break;
            case 3:
                serverUtils.PostServerData("lightarray3", value);
                break;
            case 4:
                serverUtils.PostServerData("lightarray4", value);
                break;
            case 5:
                serverUtils.PostServerData("lightarray5", value);
                break;
            case 6:
                serverUtils.PostServerData("lightarray6", value);
                break;
            case 7:
                serverUtils.PostServerData("lightarray7", value);
                break;
            case 8:
                serverUtils.PostServerData("lightarray8", value);
                break;
            case 9:
                serverUtils.PostServerData("lightarray9", value);
                break;
            case 10:
                serverUtils.PostServerData("lightarray10", value);
                break;
        }
    }

	public void PowerUp()
	{
		deployStatus = 1;
	}

	public void Launch()
	{
		deployStatus = 1;
	}

	public void buttonPress()
	{
        if (deployStatus == 3)
            return;

        if (deployStatus == 1)
        {
            light1 = false;
            Deploy1();
        }

        if (deployStatus == 2)
        {
            light2 = false;
            Deploy2();
        }

        deployStatus++;
        PostData(lightID, deployStatus);
    }

	public void DefaultState()
	{
		ObtainThemeColour();

		ReadyLeft.color = 		new Color(kColor.r, kColor.g, kColor.b, 0);
		ReadyRight.color = 		new Color(kColor.r, kColor.g, kColor.b, 0);
		ReadyHighlight.color = 	new Color(kColor.r, kColor.g, kColor.b, 0);

		this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
		
		LeftPaddle.color = 		new Color(1, 1, 1, 0.5f);
		RightPaddle.color = 	new Color(1, 1, 1, 0.5f);
		
		Title.SetText("POWER UP");
		LeftText.SetText("OFF");
		RightText.SetText("OFF");

		light1 = false;
		light2 = false;
	}

	void ReadyState()
	{
		StartCoroutine(ReadyState1());
		StartCoroutine(ReadyState2());
		StartCoroutine(ReadyState3());

        ReadyLeft.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f), 0.2f);

        LeftPaddle.DOColor(new Color(1, 1, 1, 0),0.1f);
		RightPaddle.DOColor(new Color(1, 1, 1, 0),0.1f);

		Title.SetText("");
		LeftText.SetText("");
		RightText.SetText("");
	}

    void ReadyState_2()
    {
        StartCoroutine(ReadyState1());
        StartCoroutine(Deploy1State1());
        StartCoroutine(ReadyState3());

        ReadyLeft.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f), 0.2f);
        ReadyRight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.5f), 0.2f);
        ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.0f), 0.2f);

        LeftPaddle.DOColor(new Color(1, 1, 1, 0.5f), 0.2f);
        RightPaddle.DOColor(new Color(1, 1, 1, 0), 0.1f);

        Title.SetText("");
        //LeftText.SetText("");
        RightText.SetText("");
    }

    void DeployedState()
	{
        ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.0f), 0.2f);
        ReadyLeft.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f), 0.2f);
        ReadyRight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f), 0.2f);

        LeftPaddle.DOColor(new Color(1, 1, 1, 0.5f), 0.1f);
        RightPaddle.DOColor(new Color(1, 1, 1, 0.5f), 0.1f);

        Title.SetText("LAUNCHED");
        LeftText.SetText("DEPLOYED");
        RightText.SetText("DEPLOYED");
    }

    /** Sets the left paddle to Deployed. */
    void Deploy1()
	{

        Debug.Log("Deploy state: " + deployStatus + "Launching light 1");

		LeftText.SetText("DEPLOYED");

		ReadyLeft.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f),0.2f);

		StartCoroutine(Deploy1State1());
	}


    /** Sets the right paddle to Deployed. */
    void Deploy2()
	{
        Debug.Log("Deploy state: " + deployStatus + "Launching light 2");

        RightText.SetText("DEPLOYED");
		
		ReadyRight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f),0.2f);
		
		StartCoroutine(Deploy2State1());
	}

    /** Sets the ready state to launch. */
    IEnumerator ReadyState1()
	{
		yield return new WaitForSeconds(0.1f);

		ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.3f),0.1f);
		this.GetComponent<Image>().DOColor(new Color(kColor.r, kColor.g, kColor.b, 1.0f),0.1f);

		Title.SetText("LAUNCH");

	}

    /** Sets left paddle color and text. */
    IEnumerator ReadyState2()
	{
        Debug.Log("Deploy state: " + deployStatus + "Powering up light 1");

        yield return new WaitForSeconds(0.1f);

		ReadyLeft.DOColor(new Color(kColor.r, kColor.g, kColor.b, 1),0.2f);
		LeftText.SetText("READY");
		light1 = true;

	}


    /** Sets right paddle color and text. */
    IEnumerator ReadyState3()
	{
        Debug.Log("Deploy state: " + deployStatus + "Powering up light 2");

        yield return new WaitForSeconds(0.2f);

		ReadyRight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 1),0.2f);
		RightText.SetText("READY");

	}


    /** Launches the left light, always first. */
	IEnumerator Deploy1State1()
	{
		ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.1f),0.2f);

		yield return new WaitForSeconds(0.2f);

		ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.3f),0.6f);
	
		LeftPaddle.DOColor(new Color(1, 1, 1, 0.5f),0.2f);
		light2 = true;
	
	
	}

    /** Launches the right light and sets the launched text. */
    IEnumerator Deploy2State1()
	{
		ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.0f),0.2f);
		this.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1.0f),0.1f);
		Title.SetText("LAUNCHED");

		yield return new WaitForSeconds(0.2f);

		RightPaddle.DOColor(new Color(1, 1, 1, 0.5f),0.2f);

	}

	//IEnumerator Deploy1State2()
	//{
	//	yield return new WaitForSeconds(0.2f);
	//
	//	ReadyRight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 1),0.3f);
	//	RightText.SetText("READY");
	//	light2 = true;
	//
	//}

	public void SetStatus(int _iStatus)
	{
		deployStatus = _iStatus;
        PostData(lightID, deployStatus);
	}
}
