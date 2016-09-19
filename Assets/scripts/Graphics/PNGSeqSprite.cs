using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Stage
{
    public Sprite[] sprites;
    public bool looping = false;
    public bool FallThrough = false;
    public string FolderName;
}


public class PNGSeqSprite : MonoBehaviour
{
    public Image Destination;
    int iFrame = 0;
    public int iStage = 0;
    private int iLastStage = 0;

    public bool DebugMode = false;

    public Stage[] stages;


    public float FramesPerSecond = 30f;
    int iIndex;
    int iLastIndex = 99999;

    float AnimationTimer = 0;

    float Timer = 0;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < stages.Length; ++i)
        {
            stages[i].sprites = Resources.LoadAll<Sprite>(stages[i].FolderName);
        }

        iFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugMode)
        {
            DebugStuff();
        }

        if (iLastStage != iStage)
        {
            setStage(iStage);
        }

        iIndex = (int)Mathf.Round((Timer * FramesPerSecond));
        Timer += Time.deltaTime;

        if (stages[iStage].looping)
        {
            iIndex = iIndex % stages[iStage].sprites.Length;
        }
        else
        {
            if (iIndex > stages[iStage].sprites.Length - 1)
            {
                iIndex = stages[iStage].sprites.Length - 1;
                if (stages[iStage].FallThrough)
                {
                    setStage(iStage + 1);
                }
            }
        }

        if (iIndex != iLastIndex)
        {
            Destination.sprite = stages[iStage].sprites[iIndex];
        }
        iLastIndex = iIndex;
    }

    public void setStage(int _iStage)
    {
        if (_iStage > stages.Length - 1)
        {
            _iStage = stages.Length - 1;
        }

        iStage = _iStage;
        iFrame = 0;
        Timer = 0f;

        iLastStage = iStage;
    }

    void DebugStuff()
    {
        if (Input.GetKeyDown("space"))
        {
            setStage(iStage + 1);
        }

        if (Input.GetKeyDown("escape"))
        {
            setStage(0);
        }
    }
}
