using UnityEngine;
using System.Collections;
using Meg.Networking;

public class debugShipVitals : MonoBehaviour {

    public GameObject pitchIncrease;
    public GameObject pitchDecrease;
    public GameObject yawIncrease;
    public GameObject yawDecrease;
    public GameObject rollIncrease;
    public GameObject rollDecrease;
    public GameObject veloctiyIncrease;
    public GameObject veloctiyDecrease;
    public GameObject stopButton;
    public GameObject removeInputButton;

    public float changeAmount = 2.5f;

    private float newVelocityValue;
    private float newPitchValue;
    private float newYawValue;
    private float newRollValue;
    public bool canChangeValue = true;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (pitchIncrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newPitchValue = serverUtils.GetServerData("pitchAngle") - changeAmount;
            serverUtils.SetServerData("pitchAngle", newPitchValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (pitchDecrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newPitchValue = serverUtils.GetServerData("pitchAngle") + changeAmount;
            serverUtils.SetServerData("pitchAngle", newPitchValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (yawIncrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newYawValue = serverUtils.GetServerData("yawAngle") + changeAmount;
            serverUtils.SetServerData("yawAngle", newYawValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (yawDecrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newYawValue = serverUtils.GetServerData("yawAngle") - changeAmount;
            serverUtils.SetServerData("yawAngle", newYawValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (rollIncrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newRollValue = serverUtils.GetServerData("rollAngle") + changeAmount;
            serverUtils.SetServerData("rollAngle", newRollValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (rollDecrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newRollValue = serverUtils.GetServerData("rollAngle") - changeAmount;
            serverUtils.SetServerData("rollAngle", newRollValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (veloctiyIncrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newVelocityValue = serverUtils.GetServerData("velocity") + changeAmount;
            serverUtils.SetServerData("velocity", newVelocityValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (veloctiyDecrease.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newVelocityValue = serverUtils.GetServerData("velocity") - changeAmount;
            serverUtils.SetServerData("velocity", newVelocityValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (stopButton.GetComponent<buttonControl>().pressed && canChangeValue)
        {
            newVelocityValue = 0f;
            serverUtils.SetServerData("velocity", newVelocityValue);
            canChangeValue = false;
            StartCoroutine(Wait(0.2f));
        }

        if (removeInputButton.GetComponent<buttonControl>().active)
        {
            serverUtils.SetServerData("inputXaxis", 0f);
            serverUtils.SetServerData("inputYaxis", 0f);
            serverUtils.SetServerData("inputZaxis", 0f);
            serverUtils.SetServerData("inputX2axis", 0f);
            serverUtils.SetServerData("inputY2axis", 0f);
            serverUtils.SetServerBool("disableInput", true);
            removeInputButton.GetComponent<buttonControl>().warning = true;
        }
        else
        {
            serverUtils.SetServerBool("disableInput", false);
            removeInputButton.GetComponent<buttonControl>().warning = false;
        }

    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canChangeValue = true;
    }
}
