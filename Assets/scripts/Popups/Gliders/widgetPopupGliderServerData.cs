using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class widgetPopupGliderServerData : NetworkBehaviour
{
    [SyncVar]
    public int Bootup1 = 0;

    [SyncVar]
    public int Bootup2 = 0;

    [SyncVar]
    public int Bootup3 = 0;

    [SyncVar]
    public int Bootup4 = 0;

    [SyncVar]
    public int Bootup5 = 0;
}
