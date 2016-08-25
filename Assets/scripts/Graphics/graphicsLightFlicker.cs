using UnityEngine;
using System.Collections;

public class graphicsLightFlicker : MonoBehaviour {

    public float flickerAmount = 1.0f;
    public Light l;
    public float initialIntensity = 1.0f;

	// Use this for initialization
	void Start () {
        l = gameObject.GetComponent<Light>();
        initialIntensity = l.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        l.intensity = initialIntensity;
        l.intensity += Random.Range(-flickerAmount, flickerAmount);
	}
}
