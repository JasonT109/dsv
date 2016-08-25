using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DCCTextScaler : MonoBehaviour {

    private DynamicText[] texts;
    private float[] sizes;
    private float scale;
    public bool doScale = false;

    IEnumerator reset()
    {
        yield return new WaitForSeconds(0);
        doScale = false;
    }

    void OnEnable()
    {
        texts = GetComponentsInChildren<DynamicText>();
        GetTextSizes();
    }

    void OnDisable()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].size = sizes[i];
        }
    }

    void GetTextSizes()
    {
        sizes = new float[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            sizes[i] = texts[i].size;
        }
    }

    void SetTextScale()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].transform.localScale = new Vector3(1 / scale, 1 / scale, 1 / scale);
            texts[i].size = sizes[i] * scale;
            texts[i].GenerateMesh();
        }
    }

	// Update is called once per frame
	void Update ()
    {
        scale = transform.localScale.x;

        if (doScale)
        {
            SetTextScale();
            StartCoroutine(reset());
        }
    }
}
