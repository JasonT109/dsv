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
	    var parameter = Text.text.ToLower();
	    var valid = serverUtils.InterfaceParameters.Contains(parameter);

        var prefix = valid || serverUtils.WriteableParameters.Any(p => p.StartsWith(parameter));
        var target = valid ? ValidColor : Color.Lerp(InvalidColor, ValidColor, prefix ? 0.75f : 0);

        // Indicate readonly parameters as grey.
	    if (!valid && serverUtils.GetServerDataInfo(parameter).readOnly)
	        target = Color.grey;

        Text.color = Color.Lerp(Text.color, target, Time.deltaTime / SmoothTime);
	}
}
