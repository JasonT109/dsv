using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageSlavedSequenceRawUi : MonoBehaviour
{
    public RawImage Image;

    public ImageSequenceRawUi MasterSequence;

    public string folderName;
    public string imageSequenceName;

    private string baseName;
    private int frameCounter = -1;

    void Awake()
    {
        if (!Image)
            Image = GetComponent<RawImage>();

        baseName = folderName + "/" + imageSequenceName;
    }

    private void LateUpdate()
    {
        if (frameCounter == MasterSequence.frameCounter)
            return;

        frameCounter = MasterSequence.frameCounter;
        UpdateCurrentFrame();
    }

    private int GetFrameId(int i)
        { return MasterSequence.startFrame + i; }

    private string GetFrameName(int i)
        { return baseName + GetFrameId(i).ToString("D5"); }

    private void UpdateCurrentFrame()
        { UpdateFrame(frameCounter); }

    private void UpdateFrame(int i)
    {
        var frameName = GetFrameName(i);
        if (Image)
            Image.texture = Resources.Load<Texture>(frameName);
    }

}
