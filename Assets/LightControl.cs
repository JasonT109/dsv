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

        if (iStage > 1)
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
        SBoardActive = !SBoardActive;

        if(SBoardActive)
        {
            SBoardButton.color = new Color(SBoardButton.color.r, SBoardButton.color.g, SBoardButton.color.b, 1f);
            SBoardLights.SetActive(true);
        }
        else
        {
            SBoardButton.color = new Color(SBoardButton.color.r, SBoardButton.color.g, SBoardButton.color.b, 0f);
            SBoardLights.SetActive(false);
        }
    }

    public void TogglePort()
    {
        PortActive = !PortActive;

        if (PortActive)
        {
            PortButton.color = new Color(PortButton.color.r, PortButton.color.g, PortButton.color.b, 1f);
            PortLights.SetActive(true);
        }
        else
        {
            PortButton.color = new Color(PortButton.color.r, PortButton.color.g, PortButton.color.b, 0f);
            PortLights.SetActive(false);
        }
    }

    public void ToggleBow()
    {
        BowActive = !BowActive;

        if (BowActive)
        {
            BowButton.color = new Color(BowButton.color.r, BowButton.color.g, BowButton.color.b, 1f);
            BowLights.SetActive(true);
        }
        else
        {
            BowButton.color = new Color(BowButton.color.r, BowButton.color.g, BowButton.color.b, 0f);
            BowLights.SetActive(false);
        }
    }
}
