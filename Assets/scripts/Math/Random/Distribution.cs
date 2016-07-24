using UnityEngine;
using System.Collections;

/** Yields random samples that conform to a specified distribution curve. */

[System.Serializable]
public struct Distribution
{

    // Properties
    // ------------------------------------------------------------

    /** The distribution curve (domain is assumed to be [0..1]). */
    public AnimationCurve Curve;

    /** Value range that the curve covers. */
    public Vector2 Range;


    // Methods
    // ------------------------------------------------------------

    /** Return a randomly sampled value from the distribution. */
    public float Sample(int attempts = 20)
        { return Range.x + SampleUnit(attempts) * (Range.y - Range.x); }

    /** Return a randomly sampled value from the distribution. */
    private float SampleUnit(int attempts)
    {
        // Perform rejection sampling (with limited attempts).
        for (int i = 0; i < attempts; i++)
        {
            float x = Random.value;
            float y = Random.value;
            if (y <= Curve.Evaluate(x))
                return x;
        }

        // Attempt limit was exceeded.
        return 0;
    }


}
