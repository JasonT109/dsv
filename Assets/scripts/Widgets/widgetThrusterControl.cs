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

    void OnEnable()
    {
        Update();
    }
	
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

        if (mainRPMTextL)
            mainRPMTextL.text = (Mathf.Abs(thrusterMainL * 50)).ToString("n0") + " rpm";
        if (mainRPMTextR)
            mainRPMTextR.text = (Mathf.Abs(thrusterMainR * 50)).ToString("n0") + " rpm";
        if (mainYawTextL)
            mainYawTextL.text = (thrusterVectorAngleL * 0.01f).ToString("n0") + "°";
        if (mainYawTextR)
            mainYawTextR.text = (thrusterVectorAngleR * 0.01f).ToString("n0") + "°";
        if (mainRPMTextSide1L)
            mainRPMTextSide1L.text = (Mathf.Abs(thrusterSideL1 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide2L)
            mainRPMTextSide2L.text = (Mathf.Abs(thrusterSideL2 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide3L)
            mainRPMTextSide3L.text = (Mathf.Abs(thrusterSideL3 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide1R)
            mainRPMTextSide1R.text = (Mathf.Abs(thrusterSideR1 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide2R)
            mainRPMTextSide2R.text = (Mathf.Abs(thrusterSideR2 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide3R)
            mainRPMTextSide3R.text = (Mathf.Abs(thrusterSideR3 * 50f)).ToString("n0") + " rpm";

        if (mainPowerTextSide1L)
            mainPowerTextSide1L.text = (Mathf.Abs(thrusterSideL1 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide2L)
            mainPowerTextSide2L.text = (Mathf.Abs(thrusterSideL2 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide3L)
            mainPowerTextSide3L.text = (Mathf.Abs(thrusterSideL3 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide1R)
            mainPowerTextSide1R.text = (Mathf.Abs(thrusterSideR1 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide2R)
            mainPowerTextSide2R.text = (Mathf.Abs(thrusterSideR2 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide3R)
            mainPowerTextSide3R.text = (Mathf.Abs(thrusterSideR3 * 1f)).ToString("n0") + " %";

        if (thrusterMainL >= 0)
        {
            if (tMainLPos)
                tMainLPos.GetComponent<digital_gauge>().value = (int)thrusterMainL;
            if (tMainLNeg)
                tMainLNeg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tMainLPos)
                tMainLPos.GetComponent<digital_gauge>().value = 0;
            if (tMainLNeg)
                tMainLNeg.GetComponent<digital_gauge>().value = -(int)thrusterMainL;
        }

        if (thrusterMainR >= 0)
        {
            if (tMainRPos)
                tMainRPos.GetComponent<digital_gauge>().value = (int)thrusterMainR;
            if (tMainRNeg)
                tMainRNeg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tMainRPos)
                tMainRPos.GetComponent<digital_gauge>().value = 0;
            if (tMainRNeg)
                tMainRNeg.GetComponent<digital_gauge>().value = -(int)thrusterMainR;
        }

        if (thrusterSideL1 >= 0)
        {
            if (tSideL1Pos)
                tSideL1Pos.GetComponent<digital_gauge>().value = (int)thrusterSideL1;
            if (tSideL1Neg)
                tSideL1Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tSideL1Pos)
                tSideL1Pos.GetComponent<digital_gauge>().value = 0;
            if (tSideL1Neg)
                tSideL1Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideL1;
        }

        if (thrusterSideL2 >= 0)
        {
            if (tSideL2Pos)
                tSideL2Pos.GetComponent<digital_gauge>().value = (int)thrusterSideL2;
            if (tSideL2Neg)
                tSideL2Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tSideL2Pos)
                tSideL2Pos.GetComponent<digital_gauge>().value = 0;
            if (tSideL2Neg)
                tSideL2Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideL2;
        }

        if (thrusterSideL3 >= 0)
        {
            if (tSideL3Pos)
                tSideL3Pos.GetComponent<digital_gauge>().value = (int)thrusterSideL3;
            if (tSideL3Neg)
                tSideL3Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tSideL3Pos)
                tSideL3Pos.GetComponent<digital_gauge>().value = 0;
            if (tSideL3Neg)
                tSideL3Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideL3;
        }

        if (thrusterSideR1 >= 0)
        {
            if (tSideR1Pos)
                tSideR1Pos.GetComponent<digital_gauge>().value = (int)thrusterSideR1;
            if (tSideR1Neg)
                tSideR1Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tSideR1Pos)
                tSideR1Pos.GetComponent<digital_gauge>().value = 0;
            if (tSideR1Neg)
                tSideR1Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideR1;
        }

        if (thrusterSideR2 >= 0)
        {
            if (tSideR2Pos)
                tSideR2Pos.GetComponent<digital_gauge>().value = (int)thrusterSideR2;
            if (tSideR2Neg)
                tSideR2Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tSideR2Pos)
                tSideR2Pos.GetComponent<digital_gauge>().value = 0;
            if (tSideR2Neg)
                tSideR2Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideR2;
        }

        if (thrusterSideR3 >= 0)
        {
            if (tSideR3Pos)
                tSideR3Pos.GetComponent<digital_gauge>().value = (int)thrusterSideR3;
            if (tSideR3Neg)
                tSideR3Neg.GetComponent<digital_gauge>().value = 0;
        }
        else
        {
            if (tSideR3Pos)
                tSideR3Pos.GetComponent<digital_gauge>().value = 0;
            if (tSideR3Neg)
                tSideR3Neg.GetComponent<digital_gauge>().value = -(int)thrusterSideR3;
        }
    }
}
