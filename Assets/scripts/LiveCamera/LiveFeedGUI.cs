using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LiveFeedGUI : MonoBehaviour 
{
	public int Index = 0;
	public Text ValText;
	public int MaxVal = 8;
	public bool IsMode = false;

	// Use this for initialization
	void Start () 
	{
		ValText.text = "" + (Index+1);

		if (IsMode) 
		{
			GetComponentInParent<ModeToTranslate>().TranslateMode ();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Plus()
	{
		if (Input.GetMouseButtonUp (0)) 
		{
			Index++;
			if (Index > MaxVal) 
			{
				Index = MaxVal;
			}
			ValText.text = "" + (Index+1);
			if (IsMode) 
			{
				GetComponentInParent<ModeToTranslate>().TranslateMode ();
			}
		}
	}

	public void Minus()
	{
		if (Input.GetMouseButtonUp (0)) 
		{
			Index--;
			if (Index < 0) 
			{
				Index = 0;
			}
			ValText.text = "" + (Index+1);
			if (IsMode) 
			{
				GetComponentInParent<ModeToTranslate>().TranslateMode ();
			}
		}
	}
}
