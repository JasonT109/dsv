using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class debugFileEntryUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Button for this entry. */
    public Toggle Toggle;

    /** The file info for this entry. */
    public FileInfo FileInfo
    {
        get { return _info; }
        set
        {
            _info = value;

            var label = Path.GetFileNameWithoutExtension(value.Name);
            if (InsertSpacesBetweenCaps)
                label = Regex.Replace(label, "[A-Z]+", " $0");
            if (ReplaceUnderscoresWithSpaces)
                label = Regex.Replace(label, "_", " ");
            if (UpperCase)
                label = label.ToUpper();

            Toggle.GetComponentInChildren<Text>().text = label;
        }
    }

    /** Whether entry is selected. */
    public bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            Toggle.isOn = value;
        }
    }

    public bool InsertSpacesBetweenCaps = true;
    public bool ReplaceUnderscoresWithSpaces = true;
    public bool UpperCase = true;


    // Members
    // ------------------------------------------------------------

    /** The file info for this entry. */
    private FileInfo _info;

    /** Whether entry is selected. */
    private bool _selected;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        if (!Toggle)
            Toggle = GetComponent<Toggle>();
    }


}
