using UnityEngine;
using System.Collections;

public class widgetVisSequencer : MonoBehaviour
{
    [Header ("Start button")]
    public buttonControl startButton;

    [Header ("Sequence Objects")]
    public visSequenceObject[] sequenceObjects;

    private float timer;
    private bool running;

    void ResetAll ()
    {
        for (int i = 0; i < sequenceObjects.Length; i++)
        {
            sequenceObjects[i].visObject.SetActive(false);
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

    void Update ()
    {

        if (startButton.active && !running)
            StartSequence();

        if (!startButton.active && running)
            StopSequence();

        if (running)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < sequenceObjects.Length; i++)
            {
                if (timer > sequenceObjects[i].visTime && !sequenceObjects[i].visObject.activeSelf)
                    sequenceObjects[i].visObject.SetActive(true);
            }
        }
    }
}
