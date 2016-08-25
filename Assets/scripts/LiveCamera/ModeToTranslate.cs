using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModeToTranslate : MonoBehaviour 
{
	public GameObject Mode;
	public GameObject Translator;
	public Text Translation;


	public void TranslateMode()
	{
		int mode = Mode.GetComponent<LiveFeedGUI> ().Index + 1;

		Translation.text = Translator.GetComponent<ModeTranslator> ().TranslateFromIndex (mode);
	}
}
