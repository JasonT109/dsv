using UnityEngine;
using System.Collections;
using Meg.Networking;

public class ImageSequenceSingleTexture : MonoBehaviour
{
    public enum playbackType
    {
        loop,
        once,
        hold
    }

    public playbackType type = playbackType.loop;

    //A texture object that will output the animation  
    private Texture texture;

    //With this Material object, a reference to the game object Material can be stored  
    private Material goMaterial;

    //An integer to advance frames  
    public int frameCounter = 0;

    //A string that holds the name of the folder which contains the image sequence  
    public string folderName;

    //The name of the image sequence  
    public string imageSequenceName;

    //The number of frames the animation has
    public int numberOfFrames;

    public string folderName2;
    public string imageSequenceName2;
    public int numberOfFrames2;
    public bool playing = true;
    public float switchSequenceThreshold = 0.5f;
    public float frameTime = 0.04f;
    public Texture notPlaying;
    public bool switchOnWarning = false;
    public bool switchGreaterThan = false;
    public string serverParam = "inputZaxis";
    private int nFrames;
    private int direction = 1;
    private string baseName;

    void Awake()
    {
        this.goMaterial = this.GetComponent<Renderer>().material;
        this.baseName = this.folderName + "/" + this.imageSequenceName;
        nFrames = numberOfFrames;
    }

    void Start()
    {
        texture = (Texture)Resources.Load(baseName + "00000", typeof(Texture));
    }

    void Update()
    {
        if (switchOnWarning)
        {
            var value = serverUtils.GetServerData(serverParam);
            var warning = switchGreaterThan 
                ? (value > switchSequenceThreshold) 
                : (value <= switchSequenceThreshold);

            if (warning)
            {
                this.baseName = this.folderName2 + "/" + this.imageSequenceName2;
                nFrames = numberOfFrames2;
            }
            else
            {
                this.baseName = this.folderName + "/" + this.imageSequenceName;
                nFrames = numberOfFrames;
            }
        }

        if (playing)
        {
            if (type == playbackType.loop)
            {
                StartCoroutine("PlayLoop", frameTime);
            }
            else
            {
                StartCoroutine("Play", frameTime);
            }
            goMaterial.mainTexture = this.texture;
        }
        else
        {
            if (notPlaying)
            {
                goMaterial.mainTexture = notPlaying;
            }
            else
            {
                goMaterial.mainTexture = (Texture)Resources.Load(baseName + (numberOfFrames -1), typeof(Texture));
            }
        }
        if (frameCounter >= nFrames)
        {
            frameCounter = 0;
        }
    }

    IEnumerator PlayLoop(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (direction == -1 && frameCounter > 0)
        {
            frameCounter--;
        }
        else if (direction == -1 && frameCounter == 0)
        {
            frameCounter = nFrames - 1;
        }
        else
        {
            frameCounter = (++frameCounter) % nFrames;
        }
        this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));
        StopCoroutine("PlayLoop");
    }

    IEnumerator Play(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (frameCounter < nFrames - 1)
        {
            ++frameCounter;
            this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));
        }

        if (type == playbackType.hold)
            playing = false;

        StopCoroutine("Play");
    }
}
