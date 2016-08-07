using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
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

    public Pilot GetPilot()
    {
        // Get a list of all the players
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            // if this player is the pilot
            if (players[i].GetComponent<gameInputs>().pilot)
            {
                Pilot p = new Pilot();
                p.pilotObject = players[i];
                p.pilotName = players[i].name;
                p.xinput = players[i].GetComponent<gameInputs>().outputX;
                p.yinput = players[i].GetComponent<gameInputs>().outputY;
                p.zinput = players[i].GetComponent<gameInputs>().output;
                p.xinput2 = players[i].GetComponent<gameInputs>().outputX2;
                p.yinput2 = players[i].GetComponent<gameInputs>().outputX2;
                return p;
            }
        }
        Pilot nullPilot = new Pilot();
        nullPilot.pilotName = "no pilot found";
        nullPilot.pilotObject = null;
        return nullPilot;
    }

    public Pilot GetLocalPilot()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            var gameInputs = players[i].GetComponent<gameInputs>();
            if (gameInputs.isLocalPlayer)
            {
                Pilot p = new Pilot();
                p.pilotObject = players[i];
                p.pilotName = players[i].name;
                p.xinput = players[i].GetComponent<gameInputs>().outputX;
                p.yinput = players[i].GetComponent<gameInputs>().outputY;
                p.zinput = players[i].GetComponent<gameInputs>().output;
                p.xinput2 = players[i].GetComponent<gameInputs>().outputX2;
                p.yinput2 = players[i].GetComponent<gameInputs>().outputX2;
                return p;
            }
        }

        Pilot nullPilot = new Pilot();
        nullPilot.pilotName = "no pilot found";
        nullPilot.pilotObject = null;
        return nullPilot;
    }

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
