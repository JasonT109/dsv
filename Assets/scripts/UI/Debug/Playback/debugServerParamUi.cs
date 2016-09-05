using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Networking;

public class debugServerParamUi : MonoBehaviour
{
    private const float SmoothTime = 0.25f;
    private const float ColorUpdateInterval = 0.25f;

    public Text Text;
    public Color ValidColor;
    public Color InvalidColor;

    private InputField _inputField;
    private string _prefix;
    private List<string> _tabOptions = new List<string>();
    private int _tabIndex = -1;

    private Color _targetColor;
    private bool _colorInitialized;
    private float _nextColorUpdateTime;

    private void Start()
    {
        if (!Text)
            Text = GetComponent<Text>();

        // Look for a parent input filed (used for tab completion).
        _inputField = GetComponentInParent<InputField>();
    }

	private void Update()
	{
	    var focussed = _inputField && _inputField.isFocused;
        if (focussed)
	    {
	        if (Input.GetKeyDown(KeyCode.Tab))
	            AttemptTabCompletion();
	        else if (Input.anyKeyDown && !Input.GetKey(KeyCode.LeftShift))
	            _prefix = "";
	    }

	    var needsAnUpdate = focussed || !_colorInitialized;
        if (Time.time > _nextColorUpdateTime && needsAnUpdate)
	    {
	        UpdateTargetColor();
            _nextColorUpdateTime = Time.time + ColorUpdateInterval;
	        _colorInitialized = true;
	    }

        Text.color = Color.Lerp(Text.color, _targetColor, Time.deltaTime / SmoothTime);
	}

    private void UpdateTargetColor()
    {
        var parameter = Text.text.ToLower();
        var valid = serverUtils.WriteableParameters.Contains(parameter);
        var prefix = valid || serverUtils.WriteableParameters.Any(p => p.StartsWith(parameter));

        _targetColor = valid ? ValidColor : Color.Lerp(InvalidColor, ValidColor, prefix ? 0.75f : 0);
        if (!valid && serverUtils.GetServerDataInfo(parameter).readOnly)
            _targetColor = Color.grey;
    }

    private void AttemptTabCompletion()
    {
        // Check that we're in an input field and have focus.
        if (!_inputField || !_inputField.isFocused)
            return;

        // Reset tab completion options as needed.
        if (string.IsNullOrEmpty(_prefix))
        {
            _prefix = Text.text.ToLower();
            _tabIndex = -1;
            _tabOptions = serverUtils.WriteableParameters
                .Where(p => p.StartsWith(_prefix)).ToList();
        }

        // Check if there are any options.
        var n = _tabOptions.Count;
        if (n <= 0)
        {
            _prefix = "";
            return;
        }

        // Move to next/previous tab completion option.
        if (Input.GetKey(KeyCode.LeftShift))
            _tabIndex = _tabIndex - 1;
        else
            _tabIndex = (_tabIndex + 1) % n;

        if (_tabIndex < 0)
            _tabIndex = n - 1;

        var current = _tabOptions[_tabIndex];

        // Apply change to parent input field.
        _inputField.text = current;
        Text.text = current;
    }
}
