using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class DeployLights : MonoBehaviour 
{
	private GameObject colourThemeObj;
	private Color kColor;

	public bool light1 = false;
	public bool light2 = false;
	private int oldStatus = 10; //forces a state pass
	private int deployStatus = 0;
	// 0 = default off state
	// 1 = ready to launch
	// 2 = deployed

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
	
	// Update is called once per frame
	void Update () 
	{
		if(deployStatus == oldStatus)
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
				DeployedState();
			}
			return;
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
		if(deployStatus == 0)
		{
			deployStatus = 1;
			return;
		}

		if(deployStatus == 1)
		{
			if(light1)
			{
				light1 = false;
				Deploy1();
				return;
			}
			else if(light2)
			{
				light2 = false;
				Deploy2();
			}
		}
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

		LeftPaddle.DOColor(new Color(1, 1, 1, 0),0.1f);
		RightPaddle.DOColor(new Color(1, 1, 1, 0),0.1f);

		Title.SetText("");
		LeftText.SetText("");
		RightText.SetText("");
	}

	void DeployedState()
	{
		
	}

	void Deploy1()
	{
		LeftText.SetText("DEPLOYED");

		ReadyLeft.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f),0.2f);

		StartCoroutine(Deploy1State1());
		//StartCoroutine(Deploy1State2());
	}

	void Deploy2()
	{
		RightText.SetText("DEPLOYED");
		
		ReadyRight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0f),0.2f);
		
		StartCoroutine(Deploy2State1());
		//StartCoroutine(Deploy2State1());
		//StartCoroutine(Deploy2State2());
	}

	void ObtainThemeColour()
	{
		if (colourThemeObj == null)
			colourThemeObj = GameObject.FindWithTag("ServerData");

		if(colourThemeObj)
		{
			kColor = colourThemeObj.GetComponent<graphicsColourHolder>().theme.keyColor;
		}
	}

	IEnumerator ReadyState1()
	{
		yield return new WaitForSeconds(0.1f);

		ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.3f),0.1f);
		this.GetComponent<Image>().DOColor(new Color(kColor.r, kColor.g, kColor.b, 1.0f),0.1f);

		Title.SetText("LAUNCH");

	}

	IEnumerator ReadyState2()
	{
		yield return new WaitForSeconds(0.1f);

		ReadyLeft.DOColor(new Color(kColor.r, kColor.g, kColor.b, 1),0.2f);
		LeftText.SetText("READY");
		light1 = true;

	}

	IEnumerator ReadyState3()
	{
		yield return new WaitForSeconds(0.2f);

		ReadyRight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 1),0.2f);
		RightText.SetText("READY");

	}

	IEnumerator Deploy1State1()
	{
		ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.1f),0.2f);

		yield return new WaitForSeconds(0.2f);

		ReadyHighlight.DOColor(new Color(kColor.r, kColor.g, kColor.b, 0.3f),0.6f);
	
		LeftPaddle.DOColor(new Color(1, 1, 1, 0.5f),0.2f);
		light2 = true;
	
	
	}

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
	}
}
