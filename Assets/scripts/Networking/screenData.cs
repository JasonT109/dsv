using UnityEngine;
using Meg.Networking;
using System;
using System.Collections;
using UnityEngine.Networking;

public class screenData : NetworkBehaviour
{

    // Synchronization
    // ------------------------------------------------------------

    /** Amount of screen-glitch across all screens. */
    [SyncVar]
    public float screenGlitch = 0;

    /** Time taken for screen-glitch to autodecay. */
    [SyncVar]
    public float screenGlitchAutoDecayTime = 0.1f;

    /** Whether autodecay is enabled. */
    [SyncVar]
    public bool screenGlitchAutoDecay = true;

    /** Maximum glitch delay (seconds). */
    [SyncVar]
    public float screenGlitchMaxDelay = 1.0f;

    /** Overall brightness value, applied to all screens. */
    [SyncVar]
    public float cameraBrightness = 1f;

    /** Image sequence trigger flag. */
    [SyncVar]
    public int startImageSequence = 0;

    /** Green-screen brightness [0..1] -> 0..100%. */
    [SyncVar]
    public float greenScreenBrightness = 1f;


    // Enumerations
    // ------------------------------------------------------------

    /** Possible types of screen. */
    public enum Type
    {
        Default,
        Glider1,
        Glider2,
        Glider3,
        DccControl,
        DccQuad,
        DccScreen3,
        DccScreen4,
        DccScreen5,
        DccSurface,
    }


    /** Possible screen content values. */
    public enum Content
    {
        Debug = -1,
        None = 0,
        Instruments,
        Comms,
        Lights,
        Thrusters,
        Nav,
        LifeSupport,
        Batteries,
        Oxygen,
        Diagnostics,
        SonarShort,
        SonarLong,
        Hud,
        Piloting,
        Docking,
        Rov,
        Power
    }

    // Structures
    // ------------------------------------------------------------

    /** Structure representing a screen's current or desired state. */
    [Serializable]
    public struct State
    {
        public Type Type;
        public Content Content;

        public DCCScreenID._screenID DccType
            { get { return GetDccScreenId(Type); } }

        public DCCWindow.contentID DccContent
            { get { return GetDccContentId(Content); } }

        public override bool Equals(object o)
        {
            if (!(o is State))
                return false;

            var s = (State) o;
            return Type == s.Type && Content == s.Content;
        }

        public override int GetHashCode()
            { return Type.GetHashCode() ^ Content.GetHashCode(); }

        public bool HasContent
            { get { return Content != Content.None; } }
    }


    // Static Methods
    // ------------------------------------------------------------

    /** Return a DCC screen type for the given screen state. */
    public static DCCScreenID._screenID GetDccScreenId(Type type)
    {
        switch (type)
        {
            case Type.DccControl:
                return DCCScreenID._screenID.control;
            case Type.DccQuad:
                return DCCScreenID._screenID.qaud;
            case Type.DccScreen3:
                return DCCScreenID._screenID.screen3;
            case Type.DccScreen4:
                return DCCScreenID._screenID.screen4;
            case Type.DccScreen5:
                return DCCScreenID._screenID.screen5;
            case Type.DccSurface:
                return DCCScreenID._screenID.surface;
            default:
                return DCCScreenID._screenID.control;
        }
    }

    /** Return a DCC window content id for the given content type. */
    public static DCCWindow.contentID GetDccContentId(Content content)
    {
        switch (content)
        {
            case Content.None:
                return DCCWindow.contentID.none;
            case Content.Instruments:
                return DCCWindow.contentID.instruments;
            case Content.Comms:
                return DCCWindow.contentID.comms;
            case Content.Lights:
                return DCCWindow.contentID.none;
            case Content.Thrusters:
                return DCCWindow.contentID.thrusters;
            case Content.Nav:
                return DCCWindow.contentID.nav;
            case Content.LifeSupport:
                return DCCWindow.contentID.lifesupport;
            case Content.Batteries:
                return DCCWindow.contentID.batteries;
            case Content.Oxygen:
                return DCCWindow.contentID.oxygen;
            case Content.Diagnostics:
                return DCCWindow.contentID.diagnostics;
            case Content.SonarShort:
                return DCCWindow.contentID.sonar;
            case Content.SonarLong:
                return DCCWindow.contentID.sonar;
            case Content.Hud:
                return DCCWindow.contentID.none;
            case Content.Piloting:
                return DCCWindow.contentID.piloting;
            case Content.Docking:
                return DCCWindow.contentID.none;
            case Content.Rov:
                return DCCWindow.contentID.none;
            case Content.Power:
                return DCCWindow.contentID.power;
            default:
                return DCCWindow.contentID.none;
        }
    }

}
