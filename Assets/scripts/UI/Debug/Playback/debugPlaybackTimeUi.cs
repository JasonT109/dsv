using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Meg.EventSystem;

public class debugPlaybackTimeUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Current time. */
    public Text TimeText;

    /** Current duration. */
    public Text DurationText;


    [Header("Colors")]

    /** Color for inactive time text. */
    public Color InactiveTimeTextColor;

    /** Color for active time text. */
    public Color ActiveTimeTextColor;

    /** Color for inactive duration text. */
    public Color InactiveDurationTextColor;

    /** Color for active duration text. */
    public Color ActiveDurationTextColor;

    /** The current file. */
    public megEventFile File { get { return megEventManager.Instance.File; } }



    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        UpdateUi();
    }

    /** Updating. */
    private void Update()
    {
        UpdateUi();
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Update the file UI. */
    private void UpdateUi()
    {
        var t = TimeSpan.FromSeconds(File.time);
        TimeText.color = File.running ? ActiveTimeTextColor : InactiveTimeTextColor;
        TimeText.text = string.Format("{0:00}:{1:00}:{2:00}.{3:0}",
            t.Hours, t.Minutes, t.Seconds, t.Milliseconds / 100);

        var d = TimeSpan.FromSeconds(File.endTime);
        DurationText.color = File.running ? ActiveDurationTextColor : InactiveDurationTextColor;
        DurationText.text = string.Format("/ {0:00}:{1:00}:{2:00}",
            d.Hours, d.Minutes, d.Seconds);
    }

}
