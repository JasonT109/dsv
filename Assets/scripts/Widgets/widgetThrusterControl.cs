using UnityEngine;
using System.Collections;
using Meg.Networking;

public class widgetThrusterControl : MonoBehaviour {

    public float thrusterMainL;
    public float thrusterMainR;
    public float thrusterSideL1;
    public float thrusterSideL2;
    public float thrusterSideL3;
    public float thrusterSideR1;
    public float thrusterSideR2;
    public float thrusterSideR3;

    public float thrusterVectorAngleL;
    public float thrusterVectorAngleR;

    public float maxVectorAngle = 30f;

    public GameObject tMainLPos;
    public GameObject tMainLNeg;
    public GameObject tMainRPos;
    public GameObject tMainRNeg;

    public GameObject tSideL1Pos;
    public GameObject tSideL1Neg;

    public GameObject tSideL2Pos;
    public GameObject tSideL2Neg;

    public GameObject tSideL3Pos;
    public GameObject tSideL3Neg;

    public GameObject tSideR1Pos;
    public GameObject tSideR1Neg;

    public GameObject tSideR2Pos;
    public GameObject tSideR2Neg;

    public GameObject tSideR3Pos;
    public GameObject tSideR3Neg;

    public TextMesh mainRPMTextL;
    public TextMesh mainRPMTextR;

    public TextMesh mainYawTextL;
    public TextMesh mainYawTextR;

    public TextMesh mainRPMTextSide1R;
    public TextMesh mainRPMTextSide2R;
    public TextMesh mainRPMTextSide3R;

    public TextMesh mainRPMTextSide1L;
    public TextMesh mainRPMTextSide2L;
    public TextMesh mainRPMTextSide3L;

    public TextMesh mainPowerTextSide1R;
    public TextMesh mainPowerTextSide2R;
    public TextMesh mainPowerTextSide3R;

    public TextMesh mainPowerTextSide1L;
    public TextMesh mainPowerTextSide2L;
    public TextMesh mainPowerTextSide3L;

    private float inX;
    private float inY;
    private float inZ;
    private float inX2;
	
	// Update is called once per frame
	void Update ()
    {
        //get the current input
        inX = serverUtils.GetServerData("inputXaxis") * 100f;
        inY = serverUtils.GetServerData("inputYaxis") * 100f;
        inZ = serverUtils.GetServerData("inputZaxis") * 100f;
        inX2 = serverUtils.GetServerData("inputXaxis2") * 100f;

        //calculate the thruster power based on inputs
        thrusterMainL = Mathf.Clamp(inZ + (-inX * 0.5f), -100, 100);
        thrusterMainR = Mathf.Clamp(inZ + (inX * 0.5f), -100, 100);
        thrusterSideL1 = Mathf.Clamp(-inY + (-inX * 0.5f) + inX2, -100, 100);
        thrusterSideR1 = Mathf.Clamp(-inY + (inX * 0.5f) + inX2, -100, 100);
        thrusterSideL2 = Mathf.Clamp(-inX + inX2, -100, 100);
        thrusterSideR2 = Mathf.Clamp(inX + inX2, -100, 100);
        thrusterSideL3 = Mathf.Clamp(inY + (-inX * 0.5f) + inX2, -100, 100);
        thrusterSideR3 = Mathf.Clamp(inY + (inX * 0.5f) + inX2, -100, 100);

        //assign the values to text objects
        thrusterVectorAngleL = inX * maxVectorAngle;
        thrusterVectorAngleR = inX * maxVectorAngle;
        mainRPMTextL.text = (Mathf.Abs(thrusterMainL * 50)).ToString("n0") + " rpm";
        mainRPMTextR.text = (Mathf.Abs(thrusterMainR * 50)).ToString("n0") + " rpm";
        mainYawTextL.text = (thrusterVectorAngleL * 0.01f).ToString("n0") + "°";
        mainYawTextR.text = (thrusterVectorAngleR * 0.01f).ToString("n0") + "°";
        mainRPMTextSide1L.text = (Mathf.Abs(thrusterSideL1 * 50f)).ToString("n0") + " rpm";
        mainRPMTextSide2L.text = (Mathf.Abs(thrusterSideL2 * 50f)).ToString("n0") + " rpm";
        mainRPMTextSide3L.text = (Mathf.Abs(thrusterSideL3 * 50f)).ToString("n0") + " rpm";
        mainRPMTextSide1R.text = (Mathf.Abs(thrusterSideR1 * 50f)).ToString("n0") + " rpm";
        mainRPMTextSide2R.text = (Mathf.Abs(thrusterSideR2 * 50f)).ToString("n0") + " rpm";
        mainRPMTextSide3R.text = (Mathf.Abs(thrusterSideR3 * 50f)).ToString("n0") + " rpm";

        mainPowerTextSide1L.text = (Mathf.Abs(thrusterSideL1 * 1f)).ToString("n0") + " %";
        mainPowerTextSide2L.text = (Mathf.Abs(thrusterSideL2 * 1f)).ToString("n0") + " %";
        mainPowerTextSide3L.text = (Mathf.Abs(thrusterSideL3 * 1f)).ToString("n0") + " %";
        mainPowerTextSide1R.text = (Mathf.Abs(thrusterSideR1 * 1f)).ToString("n0") + " %";
        mainPowerTextSide2R.text = (Mathf.Abs(thrusterSideR2 * 1f)).ToString("n0") + " %";
        mainPowerTextSide3R.text = (Mathf.Abs(thrusterSideR3 * 1f)).ToString("n0") + " %";


        if (thrusterMainL >= 0)
        {
            tMainLPos.GetComponent<digital_gauge>().value = (int)thrusterMainL;
            tMainLNeg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tMainLPos.GetComponent<digital_gauge>().value = 0;
            tMainLNeg.GetComponent<digital_gauge>().value = -(int)thrusterMainL;
        }

        if (thrusterMainR >= 0)
        {
            tMainRPos.GetComponent<digital_gauge>().value = (int)thrusterMainR;
            tMainRNeg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tMainRPos.GetComponent<digital_gauge>().value = 0;
            tMainRNeg.GetComponent<digital_gauge>().value = -(int)thrusterMainR;
        }

        if (thrusterSideL1 >= 0)
        {
            tSideL1Pos.GetComponent<digital_gauge>().value = (int)thrusterSideL1;
            tSideL1Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tSideL1Pos.GetComponent<digital_gauge>().value = 0;
            tSideL1Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideL1;
        }

        if (thrusterSideL2 >= 0)
        {
            tSideL2Pos.GetComponent<digital_gauge>().value = (int)thrusterSideL2;
            tSideL2Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tSideL2Pos.GetComponent<digital_gauge>().value = 0;
            tSideL2Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideL2;
        }

        if (thrusterSideL3 >= 0)
        {
            tSideL3Pos.GetComponent<digital_gauge>().value = (int)thrusterSideL3;
            tSideL3Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tSideL3Pos.GetComponent<digital_gauge>().value = 0;
            tSideL3Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideL3;
        }

        if (thrusterSideR1 >= 0)
        {
            tSideR1Pos.GetComponent<digital_gauge>().value = (int)thrusterSideR1;
            tSideR1Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tSideR1Pos.GetComponent<digital_gauge>().value = 0;
            tSideR1Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideR1;
        }

        if (thrusterSideR2 >= 0)
        {
            tSideR2Pos.GetComponent<digital_gauge>().value = (int)thrusterSideR2;
            tSideR2Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tSideR2Pos.GetComponent<digital_gauge>().value = 0;
            tSideR2Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideR2;
        }

        if (thrusterSideR3 >= 0)
        {
            tSideR3Pos.GetComponent<digital_gauge>().value = (int)thrusterSideR3;
            tSideR3Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            tSideR3Pos.GetComponent<digital_gauge>().value = 0;
            tSideR3Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideR3;
        }
    }
}
