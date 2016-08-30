using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meg.Networking;

public class serverListener : NetworkBehaviour
{

    public GameObject sData;
    public GameObject[] players;

    public struct Pilot
    {
        public GameObject pilotObject;
        public string pilotName;
        public float xinput;
        public float yinput;
        public float zinput;
        public float xinput2;
        public float yinput2;
    }

    private Pilot thePilot = new Pilot();
    private bool canSendData = true;

    private Pilot GetPilot()
    {
        // First, try to find a pilot on a remote (non-server) instance.
        // We're assuming here that the real pilot is normally on a client instance.
        var remote = GetPilotInputs().FirstOrDefault(i => !i.isLocalPlayer);
        if (remote)
            return getPilotForInput(remote);

        // If that fails, try to use the local server pilot.
        return GetLocalPilot();
    }

    private Pilot GetLocalPilot()
    {
        // Try to find a pilot on the local (server) instance.
        var local = GetPilotInputs().FirstOrDefault(i => i.isLocalPlayer);
        if (local)
            return getPilotForInput(local);

        // No pilot found.
        return new Pilot { pilotName = "None" };
    }

    private Pilot getPilotForInput(gameInputs inputs)
    {
        var p = new Pilot
        {
            pilotObject = inputs ? inputs.gameObject : null,
            pilotName = inputs.name,
            xinput = inputs.outputX,
            yinput = inputs.outputY,
            zinput = inputs.output,
            xinput2 = inputs.outputX2,
            yinput2 = inputs.outputX2
        };

        return p;
    }

    private static IEnumerable<gameInputs> GetPlayerInputs()
    {
        return GameObject.FindGameObjectsWithTag("Player")
            .Select(go => go.GetComponent<gameInputs>())
            .Where(c => c);
    }

    private static IEnumerable<gameInputs> GetPilotInputs()
        { return GetPlayerInputs().Where(i => i.pilot); }

    void ChangePilot()
    {
        sData.GetComponent<serverData>().pilot = thePilot.pilotName;
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSendData = true;
    }

    [ServerCallback]
    private void Update()
    {
        if (!isServer)
            return;

        if (sData == null)
        {
            sData = GameObject.FindWithTag("ServerData");
        }
        else
        {
            thePilot = GetPilot();
            if (thePilot.pilotObject != null && canSendData)
            {
                ChangeInput();
                ChangePilot();
                canSendData = false;
                StartCoroutine(Wait(0.02f));
            }
            else if (canSendData)
            {
                ChangePilot();
                canSendData = false;
                StartCoroutine(Wait(0.02f));
            }
        }

	}

    [Server]
    void ChangeInput()
    {
        // Apply server joystick override if needed.
        if (serverUtils.GetServerBool("joystickOverride"))
            UpdateInputFrom(GetLocalPilot());
        else if (serverUtils.GetServerBool("joystickPilot"))
            UpdateInputFrom(thePilot);
    }

    [Server]
    void UpdateInputFrom(Pilot p)
    {
        sData.GetComponent<serverData>().OnValueChanged("inputXaxis", p.xinput);
        sData.GetComponent<serverData>().OnValueChanged("inputYaxis", p.yinput);
        sData.GetComponent<serverData>().OnValueChanged("inputZaxis", p.zinput);
        sData.GetComponent<serverData>().OnValueChanged("inputXaxis2", p.xinput2);
        sData.GetComponent<serverData>().OnValueChanged("inputYaxis2", p.yinput2);
    }

}
