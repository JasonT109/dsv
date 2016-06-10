using UnityEngine;
using System.Collections;

public class heartrate : MonoBehaviour {

    public float bpm = 86.0f;
    private Renderer r;
    private float beat;
    private float tiling;

	// Use this for initialization
	void Start () {
        r = gameObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        beat = (bpm / 60) * Time.time;
        tiling = (bpm / 100) * 3;
        r.material.mainTextureOffset = new Vector2(beat, 0);
        r.material.mainTextureScale = new Vector2(tiling, 1);
    }
}
