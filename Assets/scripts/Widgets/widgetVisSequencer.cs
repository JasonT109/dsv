using UnityEngine;
using System.Collections;

public class widgetVisSequencer : MonoBehaviour
{
    [Header ("Start button (on enable if not specified)")]
    public buttonControl startButton;

    [Header ("Sequence Objects")]
    public visSequenceObject[] sequenceObjects;

    public float timer;
    private bool running;
    private bool completed;

    void ResetAll ()
    {
        completed = false;

        for (int i = 0; i < sequenceObjects.Length; i++)
        {
            if (sequenceObjects[i].visibility == visSequenceObject.visStatus.show)
                sequenceObjects[i].visObject.SetActive(false);
            //if (sequenceObjects[i].visibility == visSequenceObject.visStatus.hide)
                //sequenceObjects[i].visObject.SetActive(true);
        }
    }

    void StartSequence ()
    {
        running = true;
        timer = 0;
    }

    void StopSequence ()
    {
        running = false;
        timer = 0;
        ResetAll();
    }

    void OnEnable ()
    {
        if (!startButton)
            StartSequence();
    }

    void OnDisable ()
    {
        if (!startButton)
        {
            timer = 0;
            ResetAll();
        }
    }

    void Update ()
    {
        if (startButton)
        {
            if (startButton.active && !running && !completed)
                StartSequence();

            if (!startButton.active && running)
                StopSequence();
        }

        if (running && !completed)
        {
            timer += Time.deltaTime;

            bool isRunning = false;

            for (int i = 0; i < sequenceObjects.Length; i++)
            {
                
                if (timer > sequenceObjects[i].visTime)
                {
                    if (sequenceObjects[i].visibility == visSequenceObject.visStatus.show && !sequenceObjects[i].visObject.activeSelf)
                        sequenceObjects[i].visObject.SetActive(true);
                    if (sequenceObjects[i].visibility == visSequenceObject.visStatus.hide && sequenceObjects[i].visObject.activeSelf)
                        sequenceObjects[i].visObject.SetActive(false);
                }
                else
                {
                    isRunning = true;
                }
            }

            if (!isRunning)
            {
                completed = true;
            }
        }
    }
}
