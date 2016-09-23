using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class StageR
{
    public Texture[] textures;
    public bool looping = false;
    public bool FallThrough = false;
    public string FolderName;
}


public class PNGSeqRaw : MonoBehaviour
{
    public RawImage Destination;
    public int iStage = 0;
    private int iLastStage = 0;

    public bool DebugMode = false;

    public StageR[] stages;


    public float FramesPerSecond = 30f;
    int iIndex;
    int iLastIndex = 99999;

    float Timer = 0;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < stages.Length; ++i)
            stages[i].textures = Resources.LoadAll<Texture>(stages[i].FolderName);

        if (!Destination)
        {
            if (this.GetComponent<RawImage>())
            {
                Destination = this.GetComponent<RawImage>();
            }
        }
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
            iIndex = iIndex % stages[iStage].textures.Length;
        }
        else
        {
            if (iIndex > stages[iStage].textures.Length - 1)
            {
                iIndex = stages[iStage].textures.Length - 1;
                if (stages[iStage].FallThrough)
                {
                    setStage(iStage + 1);
                }
            }
        }

        if (iIndex != iLastIndex)
        {
            Destination.texture = stages[iStage].textures[iIndex];
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
