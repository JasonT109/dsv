using UnityEngine;
using System.Collections;
using Meg.Networking;
using UnityEngine.Networking;

public class oxygenData : NetworkBehaviour
{

    // Structures
    // ------------------------------------------------------------

    /** Sub oxygen tank configuration (capacities in litres). */
    public struct OxygenTankConfig
    {
        public int MainOxygenTanks;
        public int ReserveOxygenTanks;
        public float MainOxygenTankCapacity;
        public float ReserveOxygenTankCapacity;
    }


    // Constants
    // ------------------------------------------------------------

    /** Tank configuration for a big sub. */
    public static readonly OxygenTankConfig BigSubOxygenTankConfig = new OxygenTankConfig
    {
        MainOxygenTanks = 3,
        ReserveOxygenTanks = 6,
        MainOxygenTankCapacity = 54,
        ReserveOxygenTankCapacity = 27,
    };

    /** Tank configuration for a glider. */
    public static readonly OxygenTankConfig GliderOxygenTankConfig = new OxygenTankConfig
    {
        MainOxygenTanks = 1,
        ReserveOxygenTanks = 4,
        MainOxygenTankCapacity = 54,
        ReserveOxygenTankCapacity = 27,
    };


    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    /** Main oxygen % (total of main tanks). */
    [SyncVar]
    public float oxygen = 100;

    /** Reserve oxygen % (total of main tanks). */
    [SyncVar]
    public float reserveOxygen = 100;

    /** Oxygen flow (liters / minute, tops out to nominal 22 lpm at 5% reserves). */
    [SyncVar]
    public float oxygenFlow = 22;

    /** Main Oxygen tanks. */
    [SyncVar]
    public float oxygenTank1 = 100;
    [SyncVar]
    public float oxygenTank2 = 100;
    [SyncVar]
    public float oxygenTank3 = 100;

    /** Reserve Oxygen tanks. */
    [SyncVar]
    public float reserveOxygenTank1 = 100;
    [SyncVar]
    public float reserveOxygenTank2 = 100;
    [SyncVar]
    public float reserveOxygenTank3 = 100;
    [SyncVar]
    public float reserveOxygenTank4 = 100;
    [SyncVar]
    public float reserveOxygenTank5 = 100;
    [SyncVar]
    public float reserveOxygenTank6 = 100;


    /** Main oxygen capacity in litres (total of main tanks). */
    public float oxygenLitres
        { get { return oxygen * 0.01f * Config.MainOxygenTanks * Config.MainOxygenTankCapacity; } }

    /** Reserve oxygen capacity in litres (total of main tanks). */
    public float reserveOxygenLitres
        { get { return reserveOxygen * 0.01f * Config.ReserveOxygenTanks * Config.ReserveOxygenTankCapacity; } }


    // Unity Methods
    // ------------------------------------------------------------

    /** Server update. */
    [ServerCallback]
    public void Update()
    {
        var config = Config;

        // Update main oxygen percentage.
        oxygen = 0;
        for (var i = 0; i < config.MainOxygenTanks; i++)
            oxygen += GetMainOxygenTank(i + 1);
        oxygen /= config.MainOxygenTanks;

        // Update reserve oxygen percentage.
        reserveOxygen = 0;
        for (var i = 0; i < config.ReserveOxygenTanks; i++)
            reserveOxygen += GetReserveOxygenTank(i + 1);
        reserveOxygen /= config.ReserveOxygenTanks;
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Get main oxygen tank value (1-based.) */
    public float GetMainOxygenTank(int tank)
    {
        switch (tank)
        {
            case 1:
                return oxygenTank1;
            case 2:
                return oxygenTank2;
            case 3:
                return oxygenTank3;
            default:
                return oxygenTank1;
        }
    }

    /** Set main oxygen tank value (1-based.) */
    public void SetMainOxygenTank(int index, float value)
    {
        switch (index)
        {
            case 1:
                oxygenTank1 = value;
                break;
            case 2:
                oxygenTank2 = value;
                break;
            case 3:
                oxygenTank3 = value;
                break;
        }
    }

    /** Get reserve oxygen tank value (1-based.) */
    public float GetReserveOxygenTank(int tank)
    {
        switch (tank)
        {
            case 1:
                return reserveOxygenTank1;
            case 2:
                return reserveOxygenTank2;
            case 3:
                return reserveOxygenTank3;
            case 4:
                return reserveOxygenTank4;
            case 5:
                return reserveOxygenTank5;
            case 6:
                return reserveOxygenTank6;
            default:
                return reserveOxygenTank1;
        }
    }

    /** Set reserve oxygen tank value (1-based.) */
    public void SetReserveOxygenTank(int index, float value)
    {
        switch (index)
        {
            case 1:
                reserveOxygenTank1 = value;
                break;
            case 2:
                reserveOxygenTank2 = value;
                break;
            case 3:
                reserveOxygenTank3 = value;
                break;
            case 4:
                reserveOxygenTank4 = value;
                break;
            case 5:
                reserveOxygenTank5 = value;
                break;
            case 6:
                reserveOxygenTank6 = value;
                break;
        }
    }
    

    // Derived Properties
    // ------------------------------------------------------------

    /** Return the current sub's tank configuration. */
    public OxygenTankConfig Config
    {
        get
        {
            var vessel = serverUtils.GetPlayerVessel();
            switch (vessel)
            {
                case 1:
                case 2:
                    return BigSubOxygenTankConfig;
                case 3:
                case 4:
                    return GliderOxygenTankConfig;
                default:
                    return BigSubOxygenTankConfig;
            }
        }
    }


}
