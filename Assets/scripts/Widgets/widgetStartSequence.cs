using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetStartSequence : MonoBehaviour
{

    [Header ("Setup")]
    public ImageSequenceSingleTexture[] imageSequencers;
    public ImageSequenceSingleTexture.playbackType[] type;

    public string serverTrigger = "genericerror";
    public float serverTriggerOnValue = 1;
    public float serverTriggerOffValue = 0;
    public bool resetServerVarOnFinish = false;

    [Header ("Debug")]
    public int numberOfFrames = 100;
    public float frameLength = 0.02f;
    public float sequenceLength = 0f;
    public bool[] running;
    public float[] runFrames;

    /** Starts a sequence running. ID is required to check status of all sequences in update. */
    void EnableSequence(ImageSequenceSingleTexture sequence, int id)
    {
        sequence.type = type[id];
        sequence.gameObject.SetActive(true);
        numberOfFrames = sequence.numberOfFrames;
        frameLength = sequence.frameTime;
        sequenceLength = numberOfFrames * frameLength;
    }

    /** Stops a sequence running. ID is used to set running bool off. */
    void DisableSequence(ImageSequenceSingleTexture sequence, int id)
    {
        
        sequence.gameObject.SetActive(false);
        running[id] = false;
        runFrames[id] = 0;
        imageSequencers[id].frameCounter = 0;
    }

    void Start ()
    {
        running = new bool[imageSequencers.Length];
        runFrames = new float[imageSequencers.Length];
    }


    void Update ()
    {
	    if (serverUtils.GetServerData(serverTrigger) == serverTriggerOnValue)
        {
            for (int i = 0; i < imageSequencers.Length; i++)
            {
                if (imageSequencers[i].gameObject.activeSelf == false)
                {
                    EnableSequence(imageSequencers[i], i);
                    running[i] = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < imageSequencers.Length; i++)
            {
                DisableSequence(imageSequencers[i], i);
            }
        }

        for (int i = 0; i < running.Length; i++)
        {
            bool isStillRunning = false;

            if (running[i] && imageSequencers[i].gameObject.activeSelf)
            {
                runFrames[i] = imageSequencers[i].frameCounter;

                if (runFrames[i] == imageSequencers[i].numberOfFrames - 1)
                {
                    if (type[i] == ImageSequenceSingleTexture.playbackType.loop)
                        isStillRunning = true;
                    else
                        DisableSequence(imageSequencers[i], i);  
                }
                else
                {
                    isStillRunning = true;
                }
            }

            if (!isStillRunning && resetServerVarOnFinish)
                serverUtils.PostServerData(serverTrigger, serverTriggerOffValue);
        }
    }
}
