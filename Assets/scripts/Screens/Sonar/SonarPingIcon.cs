using UnityEngine;
using System.Collections;
using System;

public class SonarPingIcon : MonoBehaviour
{

    /** The ping who's icon this is. */
    public SonarPing Ping;

    /** The set of icon options. */
    public IconOption[] Options;


    /** An Icon option. */
    [Serializable]
    public struct IconOption
    {
        public vesselData.Icon Type;
        public GameObject Icon;
    }

    private void Start()
    {
        if (!Ping)
            Ping = transform.GetComponentInParents<SonarPing>();

        UpdateIcon();
    }
	
	private void LateUpdate()
	    { UpdateIcon(); }

    /** Update the ping's icon. */
    private void UpdateIcon()
    {
        transform.localScale = Vector3.one * Ping.Vessel.IconScale;

        var anyIconActive = false;
        for (var i = 0; i < Options.Length; i++)
        {
            var option = Options[i];
            var active = Ping.Vessel.Icon == option.Type;
            option.Icon.SetActive(active);
            anyIconActive = anyIconActive || active;
        }

        if (!anyIconActive && Options.Length > 0)
            Options[0].Icon.SetActive(true);
    }

}
