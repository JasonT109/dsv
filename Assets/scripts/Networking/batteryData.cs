using Meg.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class batteryData : NetworkBehaviour
{

    // Constants
    // ------------------------------------------------------------

    /** Number of attempts to make when selecting a bank to drain. */
    public const int DrainAttemptsPerCycle = 10;


    // Structures
    // ------------------------------------------------------------

    /** Sub battery bank configuration. */
    public struct BatteryBankConfig
    {
        /** Number of battery banks. */
        public int Banks;

        /** Overall capacity in kWh. */
        public float Life;
    }


    // Constants
    // ------------------------------------------------------------

    /** Battery configuration for a big sub. */
    public static readonly BatteryBankConfig BigSubBatteryConfig = new BatteryBankConfig
    {
        Banks = 7,
        Life = 128,
    };

    /** Battery configuration for a glider. */
    public static readonly BatteryBankConfig GliderBatteryConfig = new BatteryBankConfig
    {
        Banks = 4,
        Life = 96,
    };


    // Synchronization
    // ------------------------------------------------------------

    [Header("Synchronization")]

    [SyncVar]
    public float battery;

    /** Battery temperature (degrees c). */
    [SyncVar]
    public float batteryTemp = 5.2f;

    /** Battery current (amps). */
    [SyncVar]
    public float batteryCurrent = 12.7f;

    /** Battery life remaining (KWh). */
    [SyncVar]
    public float batteryLife = 128;

    /** Whether battery life is automatically updated. */
    [SyncVar]
    public bool batteryLifeEnabled = true;

    /** Battery life maximum (KWh). */
    [SyncVar]
    public float batteryLifeMax = 128;

    /** Battery time remaining (seconds). */
    [SyncVar]
    public float batteryTimeRemaining = 4 * 3600 + 13 * 60;

    /** Whether battery time counts down. */
    [SyncVar]
    public bool batteryTimeEnabled = true;

    /** Rate of battery drain (% per second). */
    [SyncVar]
    public float batteryDrain = 100f / (3600 * 6);

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

    /** Startup. */
    [ServerCallback]
    private void Start()
    {
        // Initialize max battery life.
        batteryLifeMax = BatteryConfiguration.Life;
    }

    /** Server update. */
    [ServerCallback]
    private void Update()
    {
        var config = BatteryConfiguration;

        // Apply battery drain to one of the banks.
        if (batteryDrain > 0)
            ApplyBatteryDrain();

        // Update battery charge percentage.
        battery = 0;
        for (var i = 0; i < config.Banks; i++)
            battery += GetBank(i + 1);
        battery /= config.Banks;

        // Update battery life remaining.
        if (batteryLifeEnabled)
            batteryLife = batteryLifeMax * (battery * 0.01f);

        // Update battery time remaining.
        if (batteryTimeEnabled)
            batteryTimeRemaining = Mathf.Max(0, batteryTimeRemaining - Time.deltaTime);
    }
    

    // Public Methods
    // ------------------------------------------------------------

    /** Get main battery tank (1-based.) */
    public float GetBank(int index)
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
    public void SetBank(int index, float value)
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


    // Private Methods
    // ------------------------------------------------------------

    /** Apply battery drain to the banks. */
    private void ApplyBatteryDrain()
    {
        // Select a random bank to drain from.
        // Skip over empty banks.
        for (var i = 0; i < DrainAttemptsPerCycle; i++)
        {
            var config = BatteryConfiguration;
            var bankToDrain = Random.Range(0, config.Banks) + 1;
            var value = GetBank(bankToDrain);
            if (value <= 0)
                continue;

            // Drain the bank of some charge.
            value = Mathf.Max(0, value - Time.deltaTime * batteryDrain);
            SetBank(bankToDrain, value);
        }
    }

}
