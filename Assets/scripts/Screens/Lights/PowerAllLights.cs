using UnityEngine;
using System.Collections;

public class PowerAllLights : MonoBehaviour 
{
	bool Powered = false;
	bool Deployed = false;

	bool AlternateSides = true;

	public GameObject[] Lights;

	private float speed = 1.0f;

	//int iDistance = 30;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void PowerUp()
	{
		if(Powered == false)
		{
			Powered = true;


			for(int i = 0; i < Lights.Length; ++i)
			{
				StartCoroutine(PowerLight(i));
			}
		}
	}

	public void AutoDeploy()
	{
		if(Deployed == false)
		{
			Deployed = true;

			if(AlternateSides)
			{
				//for(int i = 0; i < Lights.Length; ++i)
				//{
				//	StartCoroutine(DeployLeftLight(i));
				//}
				//
				//for(int i = Lights.Length; i < Lights.Length * 2; ++i)
				//{
				//	StartCoroutine(DeployRightLight(i));
				//}

				for(int i = 0; i < Lights.Length; ++i)
				{
					StartCoroutine(DeployLightAlt(i));
					StartCoroutine(DeployLightAltSBoard(i));
				}
					
			}
			else
			{
				for(int i = 0; i < Lights.Length/2; ++i)
				{
					StartCoroutine(DeployLightBoth(i));
				}

				for(int i = Lights.Length/2; i < Lights.Length/2 * 2; ++i)
				{
					StartCoroutine(DeployLightBothSBoard(i));
				}
					
			}
		}
	}

	IEnumerator PowerLight(int _i)
	{
		

		yield return new WaitForSeconds((float)_i*0.2f);

		Lights[_i].GetComponent<DeployLights>().buttonPress();

	}

	IEnumerator DeployLeftLight(int _i)
	{

		yield return new WaitForSeconds((float)_i*1.0f*speed);

		if(Lights[_i].GetComponent<DeployLights>().light1)
		{
			Lights[_i].GetComponent<DeployLights>().buttonPress();
		}

	}

	IEnumerator DeployRightLight(int _i)
	{

		yield return new WaitForSeconds((float)_i*1.0f*speed);

		if(Lights[_i-Lights.Length].GetComponent<DeployLights>().light2)
		{
			Lights[_i-Lights.Length].GetComponent<DeployLights>().buttonPress();
		}

	}

	IEnumerator DeployLightBoth(int _i)
	{

		yield return new WaitForSeconds((float)_i*1.0f*speed);

		if(Lights[_i].GetComponent<DeployLights>().light1)
		{
			Lights[_i].GetComponent<DeployLights>().buttonPress();
		}

		if(Lights[_i+5].GetComponent<DeployLights>().light1)
		{
			Lights[_i+5].GetComponent<DeployLights>().buttonPress();
		}

	}

	IEnumerator DeployLightBothSBoard(int _i)
	{

		yield return new WaitForSeconds((float)_i*1.0f*speed);


		if(Lights[_i-Lights.Length/2].GetComponent<DeployLights>().light2)
		{
			Lights[_i-Lights.Length/2].GetComponent<DeployLights>().buttonPress();
		}
			

		if(Lights[_i-Lights.Length/2+5].GetComponent<DeployLights>().light2)
		{
			Lights[_i-Lights.Length/2+5].GetComponent<DeployLights>().buttonPress();
		}

	}

	IEnumerator DeployLightAlt(int _i)
	{

		yield return new WaitForSeconds((float)_i*2.0f*speed);

		if(_i < Lights.Length/2)
		{
			if(Lights[_i].GetComponent<DeployLights>().light1)
			{
				Lights[_i].GetComponent<DeployLights>().buttonPress();
			}
		}
		else
		{
			if(Lights[_i-5].GetComponent<DeployLights>().light2)
			{
				Lights[_i-5].GetComponent<DeployLights>().buttonPress();
			}
		}
			

		//if(Lights[_i-Lights.Length/2].GetComponent<DeployLights>().light2)
		//{
		//	Lights[_i-Lights.Length/2].GetComponent<DeployLights>().buttonPress();
		//}
			

	}

	IEnumerator DeployLightAltSBoard(int _i)
	{

		yield return new WaitForSeconds((float)_i*2.0f + 1.0f*speed);

		if(_i < Lights.Length/2)
		{
			if(Lights[_i+5].GetComponent<DeployLights>().light1)
			{
				Lights[_i+5].GetComponent<DeployLights>().buttonPress();
			}
		}
		else
		{
			if(Lights[_i].GetComponent<DeployLights>().light2)
			{
				Lights[_i].GetComponent<DeployLights>().buttonPress();
			}
		}



		//if(Lights[_i-Lights.Length/2+5].GetComponent<DeployLights>().light2)
		//{
		//	Lights[_i-Lights.Length/2+5].GetComponent<DeployLights>().buttonPress();
		//}

	}

	public void SetAlt(bool _AlternateSides)
	{
		AlternateSides = _AlternateSides;
	}
		
	public void Reset()
	{
		for(int i = 0; i < Lights.Length; ++i)
		{
			Lights[i].GetComponent<DeployLights>().SetStatus(0);
		}

		Powered = false;
		Deployed = false;

		StopAllCoroutines();
	}
}
