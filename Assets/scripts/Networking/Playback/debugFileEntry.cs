using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class debugFileEntry : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Button for this entry. */
    public Button Button;

    /** The file info for this entry. */
    public FileInfo FileInfo
    {
        get { return _info; }
        set
        {
            _info = value;

            var label = Path.GetFileNameWithoutExtension(value.Name);
            label = Regex.Replace(label, "[A-Z]", " $0");
            Button.GetComponentInChildren<Text>().text = label;
        }
    }

    /** Whether entry is selected. */
    public bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            Button.Select();
        }
    }


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
        if (!Button)
            Button = GetComponent<Button>();
    }


}
