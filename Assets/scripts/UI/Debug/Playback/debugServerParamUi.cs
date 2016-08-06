using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Meg.Networking;

public class debugServerParamUi : MonoBehaviour
{
    private const float SmoothTime = 0.25f;

    public Text Text;
    public Color ValidColor;
    public Color InvalidColor;

    private void Start()
    {
        if (!Text)
            Text = GetComponent<Text>();
    }

	private void Update()
	{
	    var text = Text.text.ToLower();
	    var valid = serverUtils.Parameters.Contains(text);
	    var target = valid ? ValidColor : InvalidColor;
	    Text.color = Color.Lerp(Text.color, target, Time.deltaTime / SmoothTime);
	}
}
