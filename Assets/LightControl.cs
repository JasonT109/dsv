using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LightControl : MonoBehaviour
{
    public float LightAmountSboard;

    public float LightAmountBow;

    public float LightAmountPort;

    public PNGSeqSprite PngSeqSys;
    public GameObject Warning;

    public Image SBoardButton;
    public GameObject SBoardLights;
    public Image PortButton;
    public GameObject PortLights;
    public Image BowButton;
    public GameObject BowLights;

    private int iStage = 0;

    private bool SBoardActive = false;
    private bool PortActive = false;
    private bool BowActive = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DebugStuff();
        LightStuff();
    }

    void DebugStuff()
    {
        if (Input.GetKeyDown("space"))
        {
            PngSeqSys.setStage(iStage + 1);
        }

        if (Input.GetKeyDown("escape"))
        {
            PngSeqSys.setStage(0);
        }
    }

    void LightStuff()
    {
        iStage = PngSeqSys.iStage;

        if (iStage > 2)
        {
            Warning.SetActive(true);
        }
        else
        {
            Warning.SetActive(false);
        }
    }

    public void ToggleSBoard()
    {

    }

    public void TogglePort()
    {

    }

    public void ToggleBow()
    {

    }
}
