using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class screenData : NetworkBehaviour
{

    /** Amount of screen-glitch across all screens. */
    [SyncVar]
    public float screenGlitch = 0;

    /** Time taken for screen-glitch to autodecay. */
    [SyncVar]
    public float screenGlitchAutoDecayTime = 0.1f;

    /** Whether autodecay is enabled. */
    [SyncVar]
    public bool screenGlitchAutoDecay = true;

    [SyncVar]
    public float cameraBrightness = 1f;

    [SyncVar]
    public int startImageSequence = 0;

}
