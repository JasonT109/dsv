using UnityEngine;
using System.Collections;
using Meg.Networking;
using Meg.Maths;

public class widgetThrusterControl : MonoBehaviour
{

    public enum ThrusterId
    {
        MainL,
        MainR,
        SideL1,
        SideL2,
        SideL3,
        SideR1,
        SideR2,
        SideR3,
        MainShared
    }


    [Header("Thruster Values")]

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

    [Header("Kilowatt output")]

    public float sideThrusterOutputMin = 0f;
    public float sideThrusterOutputMax = 6.4f;
    public float mainThrusterOutputMin = 0f;
    public float mainThrusterOutputMax = 18.0f;

    [Header("Constraints")]

    public float maxVectorAngle = 30f;


    [Header("Left Thrusters")]

    public GameObject tMainLPos;
    public GameObject tMainLNeg;
    public GameObject tSideL1Pos;
    public GameObject tSideL1Neg;
    public GameObject tSideL2Pos;
    public GameObject tSideL2Neg;
    public GameObject tSideL3Pos;
    public GameObject tSideL3Neg;

    public widgetText mainRPMTextL;
    public widgetText mainPowerTextL;
    public widgetText mainYawTextL;
    public HUDLinearGauge mainYawGaugeL;

    public widgetText mainRPMTextSide1L;
    public widgetText mainPowerTextSide1L;

    public widgetText mainRPMTextSide2L;
    public widgetText mainPowerTextSide2L;

    public widgetText mainRPMTextSide3L;
    public widgetText mainPowerTextSide3L;

    [Header("Right Thrusters")]

    public GameObject tMainRPos;
    public GameObject tMainRNeg;
    public GameObject tSideR1Pos;
    public GameObject tSideR1Neg;
    public GameObject tSideR2Pos;
    public GameObject tSideR2Neg;
    public GameObject tSideR3Pos;
    public GameObject tSideR3Neg;

    public widgetText mainRPMTextR;
    public widgetText mainPowerTextR;
    public widgetText mainYawTextR;
    public HUDLinearGauge mainYawGaugeR;

    public widgetText mainRPMTextSide1R;
    public widgetText mainPowerTextSide1R;

    public widgetText mainRPMTextSide2R;
    public widgetText mainPowerTextSide2R;

    public widgetText mainRPMTextSide3R;
    public widgetText mainPowerTextSide3R;


    [Header("Appearance")]

    public Gradient LightGradient;
    public Gradient PowerGradient;

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
        inZ = (serverUtils.GetServerData("inputZaxis") * 100f) * (1 - serverUtils.GetServerData("disableinput"));
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

	    if (mainYawGaugeL)
	        mainYawGaugeL.Value = thrusterVectorAngleL * 0.01f;
        if (mainYawGaugeR)
            mainYawGaugeR.Value = thrusterVectorAngleR * 0.01f;

        if (mainRPMTextL)
            mainRPMTextL.Text = (Mathf.Abs(thrusterMainL * 50)).ToString("n0") + " rpm";
        if (mainRPMTextR)
            mainRPMTextR.Text = (Mathf.Abs(thrusterMainR * 50)).ToString("n0") + " rpm";
        if (mainYawTextL)
            mainYawTextL.Text = (thrusterVectorAngleL * 0.01f).ToString("n0") + "°";
        if (mainYawTextR)
            mainYawTextR.Text = (thrusterVectorAngleR * 0.01f).ToString("n0") + "°";
        if (mainRPMTextSide1L)
            mainRPMTextSide1L.Text = (Mathf.Abs(thrusterSideL1 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide2L)
            mainRPMTextSide2L.Text = (Mathf.Abs(thrusterSideL2 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide3L)
            mainRPMTextSide3L.Text = (Mathf.Abs(thrusterSideL3 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide1R)
            mainRPMTextSide1R.Text = (Mathf.Abs(thrusterSideR1 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide2R)
            mainRPMTextSide2R.Text = (Mathf.Abs(thrusterSideR2 * 50f)).ToString("n0") + " rpm";
        if (mainRPMTextSide3R)
            mainRPMTextSide3R.Text = (Mathf.Abs(thrusterSideR3 * 50f)).ToString("n0") + " rpm";

        if (mainPowerTextL)
            mainPowerTextL.Text = (Mathf.Abs(thrusterMainL * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide1L)
            mainPowerTextSide1L.Text = (Mathf.Abs(thrusterSideL1 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide2L)
            mainPowerTextSide2L.Text = (Mathf.Abs(thrusterSideL2 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide3L)
            mainPowerTextSide3L.Text = (Mathf.Abs(thrusterSideL3 * 1f)).ToString("n0") + " %";

        if (mainPowerTextR)
            mainPowerTextR.Text = (Mathf.Abs(thrusterMainR * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide1R)
            mainPowerTextSide1R.Text = (Mathf.Abs(thrusterSideR1 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide2R)
            mainPowerTextSide2R.Text = (Mathf.Abs(thrusterSideR2 * 1f)).ToString("n0") + " %";
        if (mainPowerTextSide3R)
            mainPowerTextSide3R.Text = (Mathf.Abs(thrusterSideR3 * 1f)).ToString("n0") + " %";

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

    public float GetThrusterPowerOutput(ThrusterId thruster)
    {
        switch (thruster)
        {
            case ThrusterId.MainL:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterMainL), 0, 100, mainThrusterOutputMin, mainThrusterOutputMax);
            case ThrusterId.MainR:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterMainR), 0, 100, mainThrusterOutputMin, mainThrusterOutputMax);
            case ThrusterId.SideL1:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterSideL1), 0, 100, sideThrusterOutputMin, sideThrusterOutputMax);
            case ThrusterId.SideL2:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterSideL2), 0, 100, sideThrusterOutputMin, sideThrusterOutputMax);
            case ThrusterId.SideL3:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterSideL3), 0, 100, sideThrusterOutputMin, sideThrusterOutputMax);
            case ThrusterId.SideR1:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterSideR1), 0, 100, sideThrusterOutputMin, sideThrusterOutputMax);
            case ThrusterId.SideR2:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterSideR2), 0, 100, sideThrusterOutputMin, sideThrusterOutputMax);
            case ThrusterId.SideR3:
                return graphicsMaths.remapValue(Mathf.Abs(thrusterSideR3), 0, 100, sideThrusterOutputMin, sideThrusterOutputMax);
            case ThrusterId.MainShared:
                return Mathf.Max(graphicsMaths.remapValue(Mathf.Abs(thrusterMainL), 0, 100, mainThrusterOutputMin, mainThrusterOutputMax), graphicsMaths.remapValue(thrusterMainR, 0, 100, mainThrusterOutputMin, mainThrusterOutputMax));
            default:
                return 0;
        }
    }


    public float GetThrusterLevel(ThrusterId thruster)
    {
        switch (thruster)
        {
            case ThrusterId.MainL:
                return thrusterMainL;
            case ThrusterId.MainR:
                return thrusterMainR;
            case ThrusterId.SideL1:
                return thrusterSideL1;
            case ThrusterId.SideL2:
                return thrusterSideL2;
            case ThrusterId.SideL3:
                return thrusterSideL3;
            case ThrusterId.SideR1:
                return thrusterSideR1;
            case ThrusterId.SideR2:
                return thrusterSideR2;
            case ThrusterId.SideR3:
                return thrusterSideR3;
            case ThrusterId.MainShared:
                return Mathf.Max(thrusterMainL, thrusterMainR);
            default:
                return 0;
        }
    }

}
