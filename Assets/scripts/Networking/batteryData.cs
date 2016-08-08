using Meg.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class batteryData : NetworkBehaviour
{

    // Structures
    // ------------------------------------------------------------

    /** Sub battery bank configuration. */
    public struct BatteryBankConfig
    {
        public int Banks;
    }


    // Constants
    // ------------------------------------------------------------

    /** Battery configuration for a big sub. */
    public static readonly BatteryBankConfig BigSubBatteryConfig = new BatteryBankConfig
    {
        Banks = 7,
    };

    /** Battery configuration for a glider. */
    public static readonly BatteryBankConfig GliderBatteryConfig = new BatteryBankConfig
    {
        Banks = 4,
    };


    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    [SyncVar]
    public float battery;

    /** Battery temperature (�c). */
    [SyncVar]
    public float batteryTemp = 5.2f;

    [SyncVar]
    public float bank1;
    [SyncVar]
    public float bank2;
    [SyncVar]
    public float bank3;
    [SyncVar]
    public float bank4;
    [SyncVar]
    public float bank5;
    [SyncVar]
    public float bank6;
    [SyncVar]
    public float bank7;


    // Unity Methods
    // ------------------------------------------------------------

    /** Server update. */
    [ServerCallback]
    public void Update()
    {
        var config = BatteryConfiguration;

        // Update battery charge percentage.
        battery = 0;
        for (var i = 0; i < config.Banks; i++)
            battery += GetMainBatteryTank(i + 1);

        battery /= config.Banks;
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Get main battery tank (1-based.) */
    public float GetMainBatteryTank(int index)
    {
        switch (index)
        {
            case 1:
                return bank1;
            case 2:
                return bank2;
            case 3:
                return bank3;
            case 4:
                return bank4;
            case 5:
                return bank5;
            case 6:
                return bank6;
            case 7:
                return bank7;
            default:
                return bank1;
        }
    }

    /** Set main battery tank (1-based.) */
    public void SetMainBatteryTank(int index, float value)
    {
        switch (index)
        {
            case 1:
                bank1 = value;
                break;
            case 2:
                bank2 = value;
                break;
            case 3:
                bank3 = value;
                break;
            case 4:
                bank4 = value;
                break;
            case 5:
                bank5 = value;
                break;
            case 6:
                bank6 = value;
                break;
            case 7:
                bank7 = value;
                break;
        }
    }


    // Derived Properties
    // ------------------------------------------------------------

    /** Return the current sub's tank configuration. */
    public BatteryBankConfig BatteryConfiguration
    {
        get
        {
            var vessel = serverUtils.GetPlayerVessel();
            if (serverUtils.IsGlider())
                return GliderBatteryConfig;

            switch (vessel)
            {
                case 1:
                case 2:
                    return BigSubBatteryConfig;
                case 3:
                case 4:
                    return GliderBatteryConfig;
                default:
                    return BigSubBatteryConfig;
            }
        }
    }


}
