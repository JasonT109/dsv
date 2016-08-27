using UnityEngine;
using System.Collections;

public class graphicsAnimateUV : MonoBehaviour
{

    public float USpeed = 0f;
    public float VSpeed = 0f;

    private Renderer r;
    public Vector2 offset = Vector2.one;

    // Use this for initialization
    void Start ()
    {
        r = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        offset.x += Time.deltaTime * USpeed;
        offset.y += Time.deltaTime * VSpeed;

        if (offset.x > 1)
            offset.x = 0 + (1 - offset.x);
        if (offset.x < 0)
            offset.x = 1 - (offset.x);

        if (offset.y > 1)
            offset.y = 0 + (1 - offset.y);
        if (offset.y < 0)
            offset.y = 1 - (offset.y);

        r.material.SetTextureOffset("_MainTex", offset);
	}
}
