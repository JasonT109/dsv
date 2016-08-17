using UnityEngine;
using System.Collections;
using Kino;
using Meg.Maths;
using Meg.Networking;

/** Applies glitch effects in response to changes in synchronized server state values. */

[RequireComponent(typeof(AnalogGlitch))]
[RequireComponent(typeof(DigitalGlitch))]

public class CameraGlitches : MonoBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Server value to monitor for glitch amount. */
    public const string GlitchKey = "screenGlitchAmount";

    /** Server value enabling glitch auto-decay. */
    public const string AutoDecayKey = "screenGlitchAutoDecay";

    /** Server value for glitch auto-decay time. */
    public const string AutoDecayTimeKey = "screenGlitchAutoDecayTime";

    /** Minimum value of normalized glitch for effect to be enabled. */
    public const float GlitchThreshold = 0.001f;


    // Public Properties
    // ------------------------------------------------------------

    [Header("Components")]

    /** Analog glitch component. */
    public AnalogGlitch Analog;

    /** Digital glitch component. */
    public DigitalGlitch Digital;


    [Header("Configuration")]

    /** Range of values that the glitch server var is expected to have. */
    public Vector2 GlitchRange = new Vector2(0, 100);

    /** Amount of scanline jitter to apply for a given noise value. */
    public AnimationCurve ScanlineJitterCurve;

    /** Amount of vertical jump to apply for a given noise value. */
    public AnimationCurve VerticalJumpCurve;

    /** Amount of horizontal shake to apply for a given noise value. */
    public AnimationCurve HorizontalShakeCurve;

    /** Amount of color drift to apply for a given noise value. */
    public AnimationCurve ColorDriftCurve;

    /** Amount of digital glitch to apply for a given noise value. */
    public AnimationCurve DigitalIntensityCurve;

    /** Whether to apply effect to the server machine. */
    public bool ApplyOnServer;


    [Header("Randomness")]

    public float ScanlineRandomness = 0.25f;
    public float VerticalJumpRandomness = 0.25f;
    public float HorizontalShakeRandomness = 0.25f;
    public float ColorDriftRandomness = 0.25f;
    public float DigitalIntensityRandomness = 0.25f;


    // Members
    // ------------------------------------------------------------

    /** Glitch smoothing velocity. */
    private float _glitchVelocity;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        if (!Analog)
            Analog = GetComponent<AnalogGlitch>();

        if (!Digital)
            Digital = GetComponent<DigitalGlitch>();

        /*
        serverUtils.RegisterServerValue(GlitchKey, new serverUtils.ParameterInfo
            { description = "The amount of digital noise on the screen.." });
        serverUtils.RegisterServerValue(AutoDecayKey, new serverUtils.ParameterInfo
            { description = "The amount of digital noise on the screen.." });
        */
    }

    /** Updating. */
    private void Update()
    {
        if (!serverUtils.IsReady())
            return;

        // Get the current glitch amount.
        var glitch = serverUtils.GetServerData(GlitchKey, 0);

        // Determine glitch parameters based on amount.
        var normalized = graphicsMaths.remapValue(glitch, GlitchRange.x, GlitchRange.y, 0, 1);
        var scanlineJitter = ScanlineJitterCurve.Evaluate(normalized);
        var verticalJump = VerticalJumpCurve.Evaluate(normalized);
        var horizontalShake = HorizontalShakeCurve.Evaluate(normalized);
        var colorDrift = ColorDriftCurve.Evaluate(normalized);
        var digitalIntensity = DigitalIntensityCurve.Evaluate(normalized);

        var isServer = serverUtils.IsServer();
        var analogEnabled = scanlineJitter > GlitchThreshold
            || verticalJump > GlitchThreshold
            || horizontalShake > GlitchThreshold
            || colorDrift > GlitchThreshold;

        if (isServer && !ApplyOnServer)
            analogEnabled = false;

        // Update analog glitch effect.
        Analog.enabled = analogEnabled;
        Analog.scanLineJitter = scanlineJitter * (1 + Random.Range(-ScanlineRandomness, ScanlineRandomness));
        Analog.verticalJump = verticalJump * (1 + Random.Range(-VerticalJumpRandomness, VerticalJumpRandomness));
        Analog.horizontalShake = horizontalShake * (1 + Random.Range(-HorizontalShakeRandomness, HorizontalShakeRandomness));
        Analog.colorDrift = colorDrift * (1 + Random.Range(-ColorDriftRandomness, ColorDriftRandomness));

        var digitalEnabled = digitalIntensity > GlitchThreshold;
        if (isServer && !ApplyOnServer)
            digitalEnabled = false;

        // Update digital glitch effect.
        Digital.enabled = digitalEnabled;
        Digital.intensity = digitalIntensity * (1 + Random.Range(-DigitalIntensityRandomness, DigitalIntensityRandomness));

        // Apply glitch auto-decay on the server (if enabled).
        var autoDecay = serverUtils.GetServerBool(AutoDecayKey);
        if (autoDecay && serverUtils.IsServer())
        {
            var autoDecayTime = serverUtils.GetServerData(AutoDecayTimeKey);
            glitch = Mathf.SmoothDamp(glitch, 0, ref _glitchVelocity, autoDecayTime);
            serverUtils.SetServerData(GlitchKey, glitch);
        }
    }

}
