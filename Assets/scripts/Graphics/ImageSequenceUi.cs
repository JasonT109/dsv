using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.UI;

public class ImageSequenceUi : MonoBehaviour
{
    public Image Image;

    public enum playbackType
    {
        loop,
        once,
        hold
    }

    [Header("Debug")]
    public int frameCounter;
    public bool playing = true;

    [Header("Configuration:")]
    public playbackType type = playbackType.loop;
    public int startFrame;
    public float frameTime = 0.04f;
    public Sprite notPlaying;

    [Header("Sequence 1")]
    public string folderName;
    public string imageSequenceName;
    public int numberOfFrames;

    [Header("Sequence 2")]
    public string folderName2;
    public string imageSequenceName2;
    public int numberOfFrames2;

    [Header("Server Switching Configuration")]
    public bool switchOnWarning = false;
    public bool switchGreaterThan = false;
    public string serverParam = "inputZaxis";
    public float switchSequenceThreshold = 0.5f;

    private int nFrames;
    private int direction = 1;
    private string baseName;

    void Awake()
    {
        if (!Image)
            Image = GetComponent<Image>();

        baseName = folderName + "/" + imageSequenceName;
        nFrames = numberOfFrames;
    }

    void Start()
    {
        frameCounter = 0;
        UpdateCurrentFrame();
    }

    void OnDisable()
    {
        if (type == playbackType.once || type == playbackType.hold)
        {
            frameCounter = 0;
            UpdateCurrentFrame();
            StopAllCoroutines();
        }
    }

    void OnEnable()
    {
        if (type == playbackType.once || type == playbackType.hold)
        {
            frameCounter = 0;
            UpdateCurrentFrame();
        }
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
                baseName = folderName2 + "/" + imageSequenceName2;
                nFrames = numberOfFrames2;
            }
            else
            {
                baseName = folderName + "/" + imageSequenceName;
                nFrames = numberOfFrames;
            }
        }

        if (playing)
        {
            if (type == playbackType.loop)
                StartCoroutine("PlayLoop", frameTime);
            else
                StartCoroutine("Play", frameTime);
        }
        else
        {
            if (notPlaying && Image)
                Image.sprite = notPlaying;
            else
                UpdateFrame(numberOfFrames - 1);
        }

        if (frameCounter >= nFrames)
            frameCounter = 0;
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

        UpdateCurrentFrame();
        StopCoroutine("PlayLoop");
    }

    IEnumerator Play(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (frameCounter < nFrames - 1)
        {
            ++frameCounter;
            UpdateCurrentFrame();
        }

        if (type == playbackType.hold)
            playing = false;

        StopCoroutine("Play");
    }

    private int GetFrameId(int i)
        { return startFrame + i; }

    private string GetFrameName(int i)
        { return baseName + GetFrameId(i).ToString("D5"); }

    private string CurrentFrame
        { get { return GetFrameName(frameCounter); } }

    private void UpdateCurrentFrame()
        { UpdateFrame(frameCounter); }

    private void UpdateFrame(int i)
    {
        var frameName = GetFrameName(i);
        if (Image)
            Image.sprite = Resources.Load<Sprite>(frameName);
    }

}
