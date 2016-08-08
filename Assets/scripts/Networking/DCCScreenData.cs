using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DCCScreenData : NetworkBehaviour
{
    [SyncVar]
    public int DCCquadScreen0 = 0;
    [SyncVar]
    public int DCCquadScreen1 = 1;
    [SyncVar]
    public int DCCquadScreen2 = 2;
    [SyncVar]
    public int DCCquadScreen3 = 3;
    [SyncVar]
    public int DCCquadScreen4 = 4;
    [SyncVar]
    public int DCCfullscreen = 0;
}
