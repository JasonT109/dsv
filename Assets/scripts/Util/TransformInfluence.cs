using UnityEngine;
using System.Collections;

/** Modifies transform of an object based on the movement of an 'influencer' object. */

public class TransformInfluence : MonoBehaviour
{

    public Transform Influencer;
    public Vector3 Translation;
    public Vector3 Scaling;
    public Vector3 Rotation;

    private struct HomeState
    {
        public Vector3 Position;
        public Vector3 Scale;
        public Vector3 Rotation;
    }

    private HomeState _home;
    private HomeState _influencerHome;

    private void Start()
    {
        _home.Position = transform.localPosition;
        _home.Rotation = transform.localRotation.eulerAngles;
        _home.Scale = transform.localScale;

        _influencerHome.Position = Influencer.localPosition;
        _influencerHome.Rotation = Influencer.localRotation.eulerAngles;
        _influencerHome.Scale = Influencer.localScale;
    }

    private void LateUpdate()
    {
        var dp = Influencer.localPosition - _influencerHome.Position;
        var dr = Influencer.localRotation.eulerAngles - _influencerHome.Rotation;
        var ds = Influencer.localScale - _influencerHome.Scale;

        transform.localPosition = _home.Position + Vector3.Scale(dp, Translation);
        transform.localRotation = Quaternion.Euler(_home.Rotation + Vector3.Scale(dr, Rotation));
        transform.localScale = _home.Scale + Vector3.Scale(ds, Scaling);
    }
}
