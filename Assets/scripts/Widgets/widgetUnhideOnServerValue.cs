using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetUnhideOnServerValue : MonoBehaviour
{
    [Header ("Server Paramerters")]
    public string serverString = "genericerror";
    public float serverValue = 0;
    public bool greaterThanValue = false;
    public bool isEqualToValue = false;
    public bool notEqualToValue = false;
    public bool inRange = false;
    public Vector2 range = new Vector2(0, 100);

    [Header("Objects to hide / show")]
    public GameObject[] objectsToUnhide;

    [Header("Unhide behaviour")]
    public bool alphaIn = false;
    public float alphaLerpInSpeed = 5;

    private bool isOn;
    private float lerpAlphaValue = 0;
    private bool isLerping;

    void SetVisibility()
    {
        bool visible = false;

        if (greaterThanValue)
        {
            if (serverUtils.GetServerData(serverString) > serverValue)
            {
                visible = true;
                if (!isOn && alphaIn)
                    isLerping = true;

                isOn = true;
            }
            else
            {
                visible = false;
                isOn = false;
            }
        }
        else if (isEqualToValue)
        {
            if (serverUtils.GetServerData(serverString) == serverValue)
            {
                visible = true;
                if (!isOn && alphaIn)
                    isLerping = true;

                isOn = true;
            }
            else
            {
                visible = false;
                isOn = false;
            }
        }
        else if (notEqualToValue)
        {
            if (serverUtils.GetServerData(serverString) != serverValue)
            {
                visible = true;
                if (!isOn && alphaIn)
                    isLerping = true;

                isOn = true;
            }
            else
            {
                visible = false;
                isOn = false;
            }
        }
        else if (inRange)
        {
            if (serverUtils.GetServerData(serverString) >= range.x && serverUtils.GetServerData(serverString) <= range.y)
            {
                visible = true;
                if (!isOn && alphaIn)
                    isLerping = true;
                isOn = true;
            }
            else
            {
                visible = false;
                isOn = false;
            }
        }
        else
        {
            if (serverUtils.GetServerData(serverString) <= serverValue)
            {
                visible = true;
                if (!isOn && alphaIn)
                    isLerping = true;
                isOn = true;
            }
            else
            {
                visible = false;
                isOn = false;
            }
        }

        for (int i = 0; i < objectsToUnhide.Length; i++)
        {
            objectsToUnhide[i].SetActive(visible);
        }
    }

    void OnEnable()
    {
        SetVisibility();
    }

    void Start ()
    {
        SetVisibility();
    }

    void SetAlphaValue()
    {
        for (int i = 0; i < objectsToUnhide.Length; i++)
        {
            Renderer r;
            r = objectsToUnhide[i].GetComponent<Renderer>();
            if (r)
                r.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, lerpAlphaValue));
        }
    }

	void Update ()
    {
        SetVisibility();
        if (isLerping)
        {
            lerpAlphaValue += Time.deltaTime * alphaLerpInSpeed;

            SetAlphaValue();

            if (lerpAlphaValue >= 1)
            {
                isLerping = false;
                lerpAlphaValue = 0;
            }
        }
    }
}
