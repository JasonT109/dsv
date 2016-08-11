using UnityEngine;
using System.Collections;

public class graphicsNoiseAnimator : MonoBehaviour {

    public float frequencySpeedMul;
    public float ampSpeedMul;
    public float offsetXMul = 1.3f;
    public float offsetYMul;
    public float tex1OffsetX = 1.0f;
    public float tex2OffsetX = 1.0f;
    public float tex1OffsetY = 0.0f;
    public float tex2OffsetY = 0.0f;

    private Renderer r;
    private Material m;

    private float intialText1OffsetX;
    private float intialText2OffsetX;
    private Vector2 newXVec1;
    private Vector2 newXVec2;

    private float intialText1OffsetY;
    private float intialText2OffsetY;


    // Use this for initialization
    void Start () {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;

        intialText1OffsetX = m.GetTextureScale("_LowTexture").x;
        intialText2OffsetX = m.GetTextureScale("_HighTexture").x;
        newXVec1 = m.GetTextureOffset("_LowTexture");
        newXVec1 = m.GetTextureOffset("_HighTexture");

        intialText1OffsetY = m.GetTextureScale("_LowTexture").y;
        intialText2OffsetY = m.GetTextureScale("_HighTexture").y;

    }
	
	// Update is called once per frame
	void Update () {
        m.SetVector("_Offset", new Vector4(offsetXMul * Time.time, offsetYMul * Time.time, 0,0));

        newXVec1.x += tex1OffsetX * Time.deltaTime;
        newXVec2.x += tex2OffsetX * Time.deltaTime;
        newXVec1.y += tex1OffsetY * Time.deltaTime;
        newXVec2.y += tex2OffsetY * Time.deltaTime;

        m.SetTextureOffset("_LowTexture", newXVec1);
        m.SetTextureOffset("_HighTexture", newXVec2);

        if (tex1OffsetX > 0)
        {
            if (newXVec1.x >= intialText1OffsetX)
            {
                float xoffset = m.GetTextureOffset("_LowTexture").x;
                int x = (int)xoffset;
                xoffset -= x;
                newXVec1.x = -(intialText1OffsetX + xoffset);
            }
        }
        else
        {
            if (newXVec1.x <= -intialText1OffsetX)
            {
                float xoffset = m.GetTextureOffset("_LowTexture").x;
                int x = (int)xoffset;
                xoffset -= x;
                newXVec1.x = (intialText1OffsetX + xoffset);
            }
        }

        if (tex2OffsetX > 0)
        {
            if (newXVec2.x >= intialText2OffsetX)
            {
                float xoffset = m.GetTextureOffset("_HighTexture").x;
                int x = (int)xoffset;
                xoffset -= x;
                newXVec2.x = -(intialText2OffsetX + xoffset);
            }
        }
        else
        {
            if (newXVec2.x <= -intialText2OffsetX)
            {
                float xoffset = m.GetTextureOffset("_HighTexture").x;
                int x = (int)xoffset;
                xoffset -= x;
                newXVec2.x = (intialText2OffsetX + xoffset);
            }
        }

        if (tex1OffsetY > 0)
        {
            if (newXVec1.y >= intialText1OffsetY)
            {
                float yoffset = m.GetTextureOffset("_LowTexture").y;
                int y = (int)yoffset;
                yoffset -= y;
                newXVec1.y = -(intialText1OffsetY + yoffset);
            }
        }
        else
        {
            if (newXVec1.y <= -intialText1OffsetY)
            {
                float yoffset = m.GetTextureOffset("_LowTexture").y;
                int y = (int)yoffset;
                yoffset -= y;
                newXVec1.y = (intialText1OffsetY + yoffset);
            }
        }

        if (tex2OffsetY > 0)
        {
            if (newXVec2.y >= intialText2OffsetY)
            {
                float yoffset = m.GetTextureOffset("_HighTexture").y;
                int y = (int)yoffset;
                yoffset -= y;
                newXVec2.y = -(intialText2OffsetY + yoffset);
            }
        }
        else
        {
            if (newXVec2.y <= -intialText2OffsetY)
            {
                float yoffset = m.GetTextureOffset("_HighTexture").y;
                int y = (int)yoffset;
                yoffset -= y;
                newXVec2.y = (intialText2OffsetY + yoffset);
            }
        }

    }
}
