using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class serverListener : NetworkBehaviour {

    public GameObject sData;
    public GameObject[] players;

    public struct pilot
    {
        public GameObject pilotObject;
        public string pilotName;
        public float xinput;
        public float yinput;
        public float zinput;
        public float xinput2;
        public float yinput2;
    }

    pilot thePilot = new pilot();
    private bool canSendData = true;

    public pilot GetPilot()
    {
        //get a list of all the players
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            //if this player is the pilot
            if (players[i].GetComponent<gameInputs>().pilot)
            {
                pilot p = new pilot();
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
        pilot nullPilot = new pilot();
        nullPilot.pilotName = "no pilot found";
        nullPilot.pilotObject = null;
        return nullPilot;
    }

    void ChangeInput()
    {
        //Debug.Log("Updating server with input");
        sData.GetComponent<serverData>().OnValueChanged("inputXaxis", thePilot.xinput);
        sData.GetComponent<serverData>().OnValueChanged("inputYaxis" , thePilot.yinput);
        sData.GetComponent<serverData>().OnValueChanged("inputZaxis", thePilot.zinput);
        sData.GetComponent<serverData>().OnValueChanged("inputXaxis2", thePilot.xinput2);
        sData.GetComponent<serverData>().OnValueChanged("inputYaxis2", thePilot.yinput2);
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

    void Update ()
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

            //Debug.Log("Pilot name: " + thePilot.pilotName + " pilot object: " + thePilot.pilotObject);

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
}
