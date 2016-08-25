using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class domeData : NetworkBehaviour
{

    /** Description for dome HUD parameters. */
    public const string HudDescription =
        "(0:None, 1:Inst, 2:Pilot 3:Hud, 4:Thrust, 5:Light, 6:Bat, 7:Map, 8:Sonar, 9:Track, 10:Comms, 11:Stats, 12:Progress)";

    // Enumerations
    // ------------------------------------------------------------

    /** Dome overlay ids. */
    public enum OverlayId
    {
        None = 0,
        Instruments = 1,
        Piloting = 2,
        Hud = 3,
        Thrusters = 4,
        Lights = 5,
        Batteries = 6,
        Map = 7,
        Sonar = 8,
        Tracking = 9,
        Comms = 10,
        DiveStats = 11,
        DiveProgress = 12
    }

    

    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    [SyncVar]
    public OverlayId domeCenter = OverlayId.None;

    [SyncVar]
    public OverlayId domeCornerBottomLeft = OverlayId.None;

    [SyncVar]
    public OverlayId domeCornerBottomRight = OverlayId.None;

    [SyncVar]
    public OverlayId domeCornerTopLeft = OverlayId.None;

    [SyncVar]
    public OverlayId domeCornerTopRight = OverlayId.None;

    [SyncVar]
    public OverlayId domeLeft = OverlayId.None;

    [SyncVar]
    public OverlayId domeHexBottomLeft = OverlayId.None;

    [SyncVar]
    public OverlayId domeHexBottomRight = OverlayId.None;

    [SyncVar]
    public OverlayId domeHexTopLeft = OverlayId.None;

    [SyncVar]
    public OverlayId domeHexTopRight = OverlayId.None;

    [SyncVar]
    public OverlayId domeRight = OverlayId.None;

    [SyncVar]
    public OverlayId domeSquareBottom = OverlayId.None;

    [SyncVar]
    public OverlayId domeSquareLeft = OverlayId.None;

    [SyncVar]
    public OverlayId domeSquareRight = OverlayId.None;

    [SyncVar]
    public OverlayId domeSquareTop = OverlayId.None;

}
