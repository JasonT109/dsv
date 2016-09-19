using UnityEngine;
using System.Collections;

public class DCCDropBucket : MonoBehaviour
{
    public enum dropBucket
    {
        left,
        middle,
        right,
        none
    }

    public dropBucket bucket = dropBucket.left;
    public DCCDropBucketManager manager;

    public Color glowColor = new Color(1f, 0.55f, 0f);
    private Color offColor;
    private Renderer r;

    void Start()
    {
        r = GetComponent<Renderer>();
        offColor = r.material.GetColor("_TintColor");
    }

    void Update()
    {
        if (manager.highlightedBucket == gameObject)
            SetGlowColor(true);
        else
            SetGlowColor(false);
    }

    public void SetGlowColor(bool on)
    {
        if (on)
            r.material.SetColor("_TintColor", glowColor);
        else
            r.material.SetColor("_TintColor", offColor);
    }
}

