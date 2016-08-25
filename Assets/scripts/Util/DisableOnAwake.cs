using UnityEngine;
using System.Collections;

public class DisableOnAwake : MonoBehaviour
{

	void Awake()
	{
	    gameObject.SetActive(false);
	}
	
}
