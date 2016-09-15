using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class DCCStrategySubmenu : MonoBehaviour
{

    /** Submenu button's on graphic. */
    public CanvasGroup On;

    /** The menu object. */
    public CanvasGroup SubMenu;

    /** Whether submenu is open. */
    public bool Opened
    {
        get { return SubMenu.gameObject.activeSelf; }
        set { SetOpened(value); }
    }

	/** Initialization. */
	private void Start()
	    { SubMenu.gameObject.SetActive(false); }

    /** Toggle submenu. */
    public void Toggle()
        { Opened = !Opened; }

    /** Open or close the submenu. */
    private void SetOpened(bool value)
    {
        if (Opened == value)
            return;

        if (value)
            Open();
        else
            Close();
    }

    /** Open the submenu. */
    public void Open()
    {
        SubMenu.gameObject.SetActive(true);
        SubMenu.transform.localScale = Vector3.zero;
        SubMenu.transform.DOScale(Vector3.one, 0.25f);
        SubMenu.alpha = 0;
        SubMenu.DOFade(1, 0.25f);

        On.alpha = 0;
        On.DOFade(1, 0.25f);
    }

    /** Close the submenu. */
    public void Close()
    {
        On.DOFade(0, 0.25f);
        SubMenu.transform.DOScale(Vector3.zero, 0.25f);
        SubMenu.DOFade(0, 0.25f).OnComplete(() => SubMenu.gameObject.SetActive(false));
    }


}
