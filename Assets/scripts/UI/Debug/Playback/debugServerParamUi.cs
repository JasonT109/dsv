using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
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
        var prefix = valid || serverUtils.Parameters.Any(p => p.StartsWith(text));
        var target = valid ? ValidColor : Color.Lerp(InvalidColor, ValidColor, prefix ? 0.75f : 0);

	    Text.color = Color.Lerp(Text.color, target, Time.deltaTime / SmoothTime);
	}
}
