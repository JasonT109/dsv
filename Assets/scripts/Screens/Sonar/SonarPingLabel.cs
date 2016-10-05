using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SonarPingLabel : MonoBehaviour
{

    /** The ping who's label this is. */
    public SonarPing Ping;

    /** The set of label options. */
    public LabelOption[] Options;


    /** An Label option. */
    [Serializable]
    public struct LabelOption
    {
        public vesselData.Label Type;
        public GameObject Label;
        public Text Name;
        public Text Description;
    }

    private void Start()
    {
        if (!Ping)
            Ping = transform.GetComponentInParents<SonarPing>();

        UpdateLabel();
    }
	
	private void LateUpdate()
	    { UpdateLabel(); }

    /** Update the ping's label. */
    private void UpdateLabel()
    {
        for (var i = 0; i < Options.Length; i++)
        {
            var option = Options[i];
            var active = Ping.Vessel.Label == option.Type;
            option.Label.SetActive(active);

            if (active && option.Name)
            {
                option.Name.text = Ping.Vessel.Name.ToUpper();
                option.Name.gameObject.SetActive(
                    !string.IsNullOrEmpty(option.Name.text));
            }

            if (active && option.Description)
            {
                option.Description.text = Ping.Vessel.Description;
                option.Description.gameObject.SetActive(
                    !string.IsNullOrEmpty(option.Description.text));
            }
        }
    }

}
