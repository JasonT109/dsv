using UnityEngine;
using Meg.Networking;

namespace Meg.Parameters
{

    /** Represents a server data parameter - part of the Setup interface backend. */

    [System.Serializable]
    public class megParameterValue : megParameter
    {

        // Properties
        // ------------------------------------------------------------

        /** The server data value to manipulate. */
        public string serverParam = "";

        /** Value to apply to server data. */
        public float serverValue
        {
            get { return GetServerData(serverParam); }
            set { PostServerData(serverParam, value, false);}
        }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        public megParameterValue(megParameterGroup group = null) : base(megParameterType.Value, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** String representation. */
        public override string ToString()
        {
            if (string.IsNullOrEmpty(serverParam))
                return base.ToString();

            var value = serverUtils.GetServerDataRaw(serverParam);
            return string.Format("{0}: {1:N1} ({2:N1})", serverParam, serverValue, value);
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("serverParam", serverParam);
            return json;
        }

        /** Load state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);
            json.GetField(ref serverParam, "serverParam");
        }

    }
}
