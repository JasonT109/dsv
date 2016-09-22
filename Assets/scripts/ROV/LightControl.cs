using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Meg.Networking;

public class LightControl : MonoBehaviour
{
    public float LightAmountSboard;

    public float LightAmountBow;

    public float LightAmountPort;

    public PNGSeqSprite PngSeqSys;
    public GameObject Warning;

    public Image SBoardButton;
    public GameObject SBoardLights;
    public Image SBoardSlider;
    public Image SBoardBeam;
    public Text SBoardText;


    public Image PortButton;
    public GameObject PortLights;
    public Image PortSlider;
    public Image PortBeam;
    public Text PortText;


    public Image BowButton;
    public GameObject BowLights;
    public Image BowSlider;
    public Image BowBeam;
    public Text BowText;


    private int iStage = 50;

    private bool SBoardActive = false;
    private bool PortActive = false;
    private bool BowActive = false;

    public bool DebugMode = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DebugMode)
        {
            DebugStuff();
        }

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
        if (PngSeqSys.iStage != iStage)
        {
            // the stage has changed. Initialise the current stage
            if (PngSeqSys.iStage == 0)
            {
                SBoardButton.color = new Color(SBoardButton.color.r, SBoardButton.color.g, SBoardButton.color.b, 0f);
                SBoardLights.SetActive(false);

                PortButton.color = new Color(PortButton.color.r, PortButton.color.g, PortButton.color.b, 0f);
                PortLights.SetActive(false);

                BowButton.color = new Color(BowButton.color.r, BowButton.color.g, BowButton.color.b, 0f);
                BowLights.SetActive(false);

                SBoardButton.gameObject.SetActive(false);
                PortButton.gameObject.SetActive(false);
                BowButton.gameObject.SetActive(false);
            }

            if (PngSeqSys.iStage == 1)
            {
                SBoardButton.color = new Color(SBoardButton.color.r, SBoardButton.color.g, SBoardButton.color.b, 0.5f);
                SBoardLights.SetActive(false);

                PortButton.color = new Color(PortButton.color.r, PortButton.color.g, PortButton.color.b, 0.5f);
                PortLights.SetActive(false);

                BowButton.color = new Color(BowButton.color.r, BowButton.color.g, BowButton.color.b, 0.5f);
                BowLights.SetActive(false);

                SBoardButton.gameObject.SetActive(true);
                PortButton.gameObject.SetActive(true);
                BowButton.gameObject.SetActive(true);
            }

        }

        UpdateBowLights();

        UpdatePortLights();

        UpdateSboardLights();

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

        if (SBoardActive)
        {
            SBoardButton.color = new Color(SBoardButton.color.r, SBoardButton.color.g, SBoardButton.color.b, 1f);
            SBoardLights.SetActive(true);
        }
        else
        {
            SBoardButton.color = new Color(SBoardButton.color.r, SBoardButton.color.g, SBoardButton.color.b, 0.5f);
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
            PortButton.color = new Color(PortButton.color.r, PortButton.color.g, PortButton.color.b, 0.5f);
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
            BowButton.color = new Color(BowButton.color.r, BowButton.color.g, BowButton.color.b, 0.5f);
            BowLights.SetActive(false);
        }
    }

    public void UpdateBowLights()
    {
        LightAmountBow = BowSlider.GetComponent<RadialSlider>().value;

        BowSlider.fillAmount = LightAmountBow;
        BowSlider.color = new Color(BowSlider.color.r, BowSlider.color.g, BowSlider.color.b, Mathf.Clamp(LightAmountBow, 0.3f, 1f));
        BowBeam.color = new Color(BowBeam.color.r, BowBeam.color.g, BowBeam.color.b, LightAmountBow);
        BowText.text = (LightAmountBow * 100f).ToString("N0");
    }

    public void UpdateSboardLights()
    {
        LightAmountSboard = SBoardSlider.GetComponent<RadialSlider>().value;

        SBoardSlider.fillAmount = LightAmountSboard;
        SBoardSlider.color = new Color(SBoardSlider.color.r, SBoardSlider.color.g, SBoardSlider.color.b, Mathf.Clamp(LightAmountSboard, 0.3f, 1f));
        SBoardBeam.color = new Color(SBoardBeam.color.r, SBoardBeam.color.g, SBoardBeam.color.b, LightAmountSboard);
        SBoardText.text = (LightAmountSboard * 100f).ToString("N0");
    }

    public void UpdatePortLights()
    {
        LightAmountPort = PortSlider.GetComponent<RadialSlider>().value;

        PortSlider.fillAmount = LightAmountPort;
        PortSlider.color = new Color(PortSlider.color.r, PortSlider.color.g, PortSlider.color.b, Mathf.Clamp(LightAmountPort, 0.3f, 1f));
        PortBeam.color = new Color(PortBeam.color.r, PortBeam.color.g, PortBeam.color.b, LightAmountPort);
        PortText.text = (LightAmountPort * 100f).ToString("N0");
    }

    public void SetState(int _iState)
    {
        PngSeqSys.setStage(_iState);
    }

    public int getPNGStage()
    {
        return (PngSeqSys.iStage);
    }
}
