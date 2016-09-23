using UnityEngine;
using Meg.Networking;
using System;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class screenData : NetworkBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** State that represents 'no state'. */
    public static readonly State NoState = new State { Type = Type.Default, Content = Content.None };


    // Static Properties
    // ------------------------------------------------------------

    /** Initial screen state on local instance. */
    public static State InitialState;


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
        GliderLeft,
        GliderMid,
        GliderRight,
        DccControl,
        DccQuad,
        DccScreen3,
        DccScreen4,
        DccScreen5,
        DccSurface,
        DccStrategy,
        Rov,
        EvacLeft,
        EvacMid,
        EvacRight,
        EvacTop,
        EvacMap
    }


    /** Possible screen content values. */
    public enum Content
    {
        Any = -2,
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
        Power,
        Controls,
        TCAS,
        Radar,
        Towing,
        Systems,
        Map,
        Cameras,
        Vitals
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

        public bool Matches(State s)
        {
            if (Content == Content.Any || s.Content == Content.Any)
                return Type == s.Type;

            return Equals(s, this);
        }

        public bool HasContent
            { get { return Content != Content.None; } }

        public int GliderScreenId
        {
            get
            {
                switch (Type)
                {
                    case Type.GliderLeft:
                        return 2;
                    case Type.GliderMid:
                        return 1;
                    case Type.GliderRight:
                        return 0;
                    default:
                        return 1;
                }
            }
        }

    }

    /** Structure identifying an on-screen window. */
    [Serializable]
    public struct WindowId
    {
        /** Type of screen this window appears on, and content that this window represents. */
        public State State;

        public override bool Equals(object o)
        {
            if (!(o is WindowId))
                return false;

            var window = (WindowId) o;
            return Equals(State, window.State);
        }

        public override int GetHashCode()
            { return State.GetHashCode(); }
    }


    /** A synchronized list of screen windows. */
    public class SyncListWindowIds : SyncListStruct<WindowId> { }



    // Static Methods
    // ------------------------------------------------------------

    /** Determine if a given screen is a DCC screen. */
    public static bool IsDccScreen(Type type)
    {
        switch (type)
        {
            case Type.DccControl:
            case Type.DccQuad:
            case Type.DccScreen3:
            case Type.DccScreen4:
            case Type.DccScreen5:
            case Type.DccSurface:
            case Type.DccStrategy:
                return true;
            default:
                return false;
        }
    }

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
            case Type.DccStrategy:
                return DCCScreenID._screenID.strategy;
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

    /** Return string representation of the given screen type. */
    public static string NameForType(Type type)
    {
        switch (type)
        {
            case Type.DccScreen3:
                return "DccTopLeft";
            case Type.DccScreen4:
                return "DccTopMid";
            case Type.DccScreen5:
                return "DccTopRight";
            default:
                var result = Enum.GetName(typeof(Type), type);
                return result ?? "";
        }
    }

    /** Return string representation of the given screen content. */
    public static string NameForContent(Content content)
    {
        var result = Enum.GetName(typeof(Content), content);
        return result ?? "";
    }

}
