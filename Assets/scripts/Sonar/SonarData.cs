using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SonarData : NetworkBehaviour 
{
    public int SonarRange;
    public float SonarGain;
    public float DefaultScale;

    [SyncVar]
    public float MegSpeed;
    [SyncVar]
    public float MegTurnSpeed;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public float getScaleSpeed()
    {
        return(MegSpeed / SonarRange * 30f);
    }

    public float getScaleTurnSpeed()
    {
        //return(MegTurnSpeed / SonarRange * 30f);
        return(MegTurnSpeed);
    }

    public float getScale()
    {
        return(DefaultScale / SonarRange * 30);
    }
}
