using UnityEngine;
using System.Collections;
using DG.Tweening;
using Meg.Networking;

public class SonarProximityAlert : MonoBehaviour
{
    /** The proximity alert's renderer. */
    public Renderer Renderer;

    /** Proximity alert's animation duration. */
    public float Duration = 0.25f;

    /** Minimum alpha for alert. */
    public float MinAlpha = 0;

    /** Initialization. */
	private void Start()
	{
	    Renderer.enabled = false;
	    Renderer.material.DOFade(MinAlpha, "_TintColor", Duration).SetLoops(-1, LoopType.Yoyo);
	    // Renderer.transform.DOPunchScale(Vector3.one * -0.05f, Duration).SetLoops(-1, LoopType.Yoyo);
	}
	
    /** Updating. */
	private void Update()
    {
        Renderer.enabled = serverUtils.GetServerData("sonarProximity") < 1;
    }

}
