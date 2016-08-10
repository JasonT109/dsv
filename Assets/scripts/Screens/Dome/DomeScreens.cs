using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DomeScreens : MonoBehaviour
{

    // Components
    // ------------------------------------------------------------

    [Header("Components")]

    /** The dome screen collection. */
    public List<DomeScreen> Screens;


    [Header("Animation")]

    /** Delay distribution for turning each successive screen on. */
    public Distribution DelayBetweenScreens;


    [Header("Presets")]

    /** Preset one. */
    public ScreenPreset[] PresetOne;

    /** Preset two. */
    public ScreenPreset[] PresetTwo;


    // Structures
    // ------------------------------------------------------------

    [System.Serializable]
    public struct ScreenPreset
    {
        public DomeScreen Screen;
        public DomeOverlayIcon Icon;
    }


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Turn all screens on. */
    public void TurnOnAll()
    {
        StopAllCoroutines();
        StartCoroutine(TurnOnAllRoutine());
    }

    /** Turn all screens on. */
    public void TurnOffAll()
    {
        StopAllCoroutines();
        StartCoroutine(TurnOffAllRoutine());
    }

    /** Select a screen preset. */
    public void ApplyPresetOne()
        { ApplyPresets(PresetOne); }

    /** Select a screen preset. */
    public void ApplyPresetTwo()
        { ApplyPresets(PresetTwo); }

    /** Clear all overlays. */
    public void ClearOverlays()
    {
        foreach (var screen in Screens)
            screen.Clear();
    }

    /** Apply screen presets. */
    public void ApplyPresets(ScreenPreset[] presets)
    {
        foreach (var screen in Screens)
        {
            screen.On = false;
            screen.Clear();
        }
        foreach (var preset in presets)
        {
            preset.Screen.On = true;
            preset.Screen.Current = preset.Icon.Overlay;
        }
    }


    // Private Methods
    // ------------------------------------------------------------

    /** Routine to turn on all screens. */
    private IEnumerator TurnOnAllRoutine()
    {
        foreach (var screen in Screens)
        {
            screen.On = true;
            var delay = DelayBetweenScreens.Sample();
            yield return new WaitForSeconds(delay);
        }
    }

    /** Routine to turn off all screens. */
    private IEnumerator TurnOffAllRoutine()
    {
        for (var i = Screens.Count - 1; i >= 0; i--)
        {
            var screen = Screens[i];
            screen.On = false;
            var delay = DelayBetweenScreens.Sample();
            yield return new WaitForSeconds(delay);
        }
    }

}
