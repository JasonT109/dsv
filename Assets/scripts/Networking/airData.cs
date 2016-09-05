using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class airData : NetworkBehaviour
{

    // Structures
    // ------------------------------------------------------------

    /** Sub oxygen tank configuration (capacities in litres). */
    public struct AirTankConfig
    {
        public int MainAirTanks;
        public int ReserveAirTanks;
        public float MainAirTankCapacity;
        public float ReserveAirTankCapacity;
    }


    // Constants
    // ------------------------------------------------------------

    /** Tank configuration for a big sub. */
    public static readonly AirTankConfig BigSubAirConfig = new AirTankConfig
    {
        MainAirTanks = 4,
        ReserveAirTanks = 9,
        MainAirTankCapacity = 54,
        ReserveAirTankCapacity = 27,
    };

    /** Tank configuration for a glider. */
    public static readonly AirTankConfig GliderAirConfig = new AirTankConfig
    {
        MainAirTanks = 2,
        ReserveAirTanks = 6,
        MainAirTankCapacity = 54,
        ReserveAirTankCapacity = 27,
    };


    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Main air % (total of main tanks). */
    [SyncVar]
    public float air = 100;

    /** Reserve air % (total of main tanks). */
    [SyncVar]
    public float reserveAir = 100;

    /** Main air tanks. */
    [SyncVar]
    public float airTank1 = 100;
    [SyncVar]
    public float airTank2 = 100;
    [SyncVar]
    public float airTank3 = 100;
    [SyncVar]
    public float airTank4 = 100;

    /** Reserve air tanks. */
    [SyncVar]
    public float reserveAirTank1 = 100;
    [SyncVar]
    public float reserveAirTank2 = 100;
    [SyncVar]
    public float reserveAirTank3 = 100;
    [SyncVar]
    public float reserveAirTank4 = 100;
    [SyncVar]
    public float reserveAirTank5 = 100;
    [SyncVar]
    public float reserveAirTank6 = 100;
    [SyncVar]
    public float reserveAirTank7 = 100;
    [SyncVar]
    public float reserveAirTank8 = 100;
    [SyncVar]
    public float reserveAirTank9 = 100;


    /** Main air capacity in litres (total of main tanks). */
    public float airLitres
        { get { return air * 0.01f * Config.MainAirTanks * Config.MainAirTankCapacity; } }
    
    /** Reserve air capacity in litres (total of main tanks). */
    public float reserveAirLitres
        { get { return reserveAir * 0.01f * Config.ReserveAirTanks * Config.ReserveAirTankCapacity; } }


    // Unity Methods
    // ------------------------------------------------------------

    /** Server update. */
    [ServerCallback]
    public void Update()
    {
        var tanks = Config;

        // Update main air percentage.
        air = 0;
        for (var i = 0; i < tanks.MainAirTanks; i++)
            air += GetMainAirTank(i + 1);
        air /= tanks.MainAirTanks;

        // Update reserve air percentage.
        reserveAir = 0;
        for (var i = 0; i < tanks.ReserveAirTanks; i++)
            reserveAir += GetReserveAirTank(i + 1);
        reserveAir /= tanks.ReserveAirTanks;
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Get main air tank (1-based.) */
    public float GetMainAirTank(int index)
    {
        switch (index)
        {
            case 1:
                return airTank1;
            case 2:
                return airTank2;
            case 3:
                return airTank3;
            case 4:
                return airTank4;
            default:
                return airTank1;
        }
    }

    /** Set main air tank (1-based.) */
    public void SetMainAirTank(int index, float value)
    {
        switch (index)
        {
            case 1:
                airTank1 = value;
                break;
            case 2:
                airTank2 = value;
                break;
            case 3:
                airTank3 = value;
                break;
            case 4:
                airTank4 = value;
                break;
        }
    }

    /** Get reserve air tank value (1-based.) */
    public float GetReserveAirTank(int tank)
    {
        switch (tank)
        {
            case 1:
                return reserveAirTank1;
            case 2:
                return reserveAirTank2;
            case 3:
                return reserveAirTank3;
            case 4:
                return reserveAirTank4;
            case 5:
                return reserveAirTank5;
            case 6:
                return reserveAirTank6;
            case 7:
                return reserveAirTank7;
            case 8:
                return reserveAirTank8;
            case 9:
                return reserveAirTank9;
            default:
                return reserveAirTank1;
        }
    }

    /** Set reserve air tank value (1-based.) */
    public void SetReserveAirTank(int index, float value)
    {
        switch (index)
        {
            case 1:
                reserveAirTank1 = value;
                break;
            case 2:
                reserveAirTank2 = value;
                break;
            case 3:
                reserveAirTank3 = value;
                break;
            case 4:
                reserveAirTank4 = value;
                break;
            case 5:
                reserveAirTank5 = value;
                break;
            case 6:
                reserveAirTank6 = value;
                break;
            case 7:
                reserveAirTank7 = value;
                break;
            case 8:
                reserveAirTank8 = value;
                break;
            case 9:
                reserveAirTank9 = value;
                break;
        }
    }


    // Derived Properties
    // ------------------------------------------------------------

    /** Return the current sub's tank configuration. */
    public AirTankConfig Config
    {
        get
        {
            var vessel = serverUtils.GetPlayerVessel();
            switch (vessel)
            {
                case 1:
                case 2:
                    return BigSubAirConfig;
                case 3:
                case 4:
                    return GliderAirConfig;
                default:
                    return BigSubAirConfig;
            }
        }
    }


}
