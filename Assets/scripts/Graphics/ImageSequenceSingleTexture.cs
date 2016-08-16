using UnityEngine;
using System.Collections;
using Meg.Networking;

public class ImageSequenceSingleTexture : MonoBehaviour
{
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

    public float switchSequenceThreshold = 0.5f;

    public float frameTime = 0.04f;
    public Texture notPlaying;

    public bool speedFromServer = false;
    public bool switchOnWarning = false;
    public bool switchGreaterThan = false;
    public string serverParam = "inputZaxis";
    public float incomingMinValue = 0f;
    public float incomingMaxValue = 1f;
    public float speedMinValue = 0.05f;
    public float speedMaxValue = 0.01f;
    public float fastSpeedMul = 1.0f;
    public float slowSpeedMul = 1.0f;
    private float speedMul = 1.0f;
    private bool usingFaster = false;
    private int nFrames;
    private int direction = 1;

    //The base name of the files of the sequence
    private string baseName;

    void Awake()
    {
        //Get a reference to the Material of the game object this script is attached to  
        this.goMaterial = this.GetComponent<Renderer>().material;
        //With the folder name and the sequence name, get the full path of the images (without the numbers)  
        this.baseName = this.folderName + "/" + this.imageSequenceName;
        nFrames = numberOfFrames;
    }

    void Start()
    {
        //set the initial frame as the first texture. Load it from the first image on the folder  
        texture = (Texture)Resources.Load(baseName + "00000", typeof(Texture));
    }

    public float GetMappedValue(float inputValue)
    {
        float returnValue;
        returnValue = (inputValue - incomingMinValue) / (incomingMaxValue - incomingMinValue) * (speedMaxValue - speedMinValue) + speedMinValue;
        return returnValue;
    }

    void Update()
    {
        if (speedFromServer)
        {
            if (serverParam == "inputZaxis")
            {
                frameTime = GetMappedValue(Mathf.Abs(serverUtils.GetServerData("inputZaxis")));
                if (serverUtils.GetServerData("inputZaxis") >= 0)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
                if (Mathf.Abs(serverUtils.GetServerData("inputZaxis")) >= switchSequenceThreshold && !usingFaster)
                {
                    //switch to faster sequence
                    speedMul = fastSpeedMul;
                    usingFaster = true;
                    this.baseName = this.folderName2 + "/" + this.imageSequenceName2;
                    nFrames = numberOfFrames2;

                }
                else if (Mathf.Abs(serverUtils.GetServerData("inputZaxis")) < switchSequenceThreshold && usingFaster)
                {
                    //use slower sequence
                    speedMul = slowSpeedMul;
                    usingFaster = false;
                    this.baseName = this.folderName + "/" + this.imageSequenceName;
                    nFrames = numberOfFrames;
                }
            }
        }

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

        if (frameTime < speedMinValue)
        {
            //Start the 'PlayLoop' method as a coroutine with a 0.04 delay  
            StartCoroutine("PlayLoop", frameTime * speedMul);
            //Set the material's texture to the current value of the frameCounter variable  
            goMaterial.mainTexture = this.texture;
        }
        else
        {
            if (notPlaying)
            {
                goMaterial.mainTexture = notPlaying;
            }
        }
        if (frameCounter >= nFrames)
        {
            frameCounter = 0;
        }
    }

    //The following methods return a IEnumerator so they can be yielded:  
    //A method to play the animation in a loop  
    IEnumerator PlayLoop(float delay)
    {
        //wait for the time defined at the delay parameter  
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
            //advance one frame
            frameCounter = (++frameCounter) % nFrames;
        }

        //load the current frame  
        this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));

        //Stop this coroutine  
        StopCoroutine("PlayLoop");
    }

    //A method to play the animation just once  
    IEnumerator Play(float delay)
    {
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);

        //if it isn't the last frame  
        if (frameCounter < nFrames - 1)
        {
            //Advance one frame  
            ++frameCounter;

            //load the current frame  
            this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D5"), typeof(Texture));
        }

        //Stop this coroutine  
        StopCoroutine("Play");
    }
}
