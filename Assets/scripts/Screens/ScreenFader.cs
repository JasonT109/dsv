using UnityEngine;
using System.Collections;

public class ScreenFader : Singleton<ScreenFader>
{
    public const float FadeDuration = 0.25f;

    public AnimateFade Fader;

    public void Fade()
    {
        Fader.Fade(FadeDuration, 0);
    }

}
