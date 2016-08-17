using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class debugFolderEntryUi : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Button for this entry. */
    public Toggle Toggle;

    /** The directory info for this entry. */
    public DirectoryInfo DirectoryInfo
    {
        get { return _info; }
        set
        {
            _info = value;

            var label = value.Name;
            if (InsertSpacesBetweenCaps)
                label = Regex.Replace(value.Name, "[A-Z]+", " $0");
            if (ReplaceUnderscoresWithSpaces)
                label = Regex.Replace(label, "_", " ");

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

    // Members
    // ------------------------------------------------------------

    /** The folder info for this entry. */
    private DirectoryInfo _info;

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
