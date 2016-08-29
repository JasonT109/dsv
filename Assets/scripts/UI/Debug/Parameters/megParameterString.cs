using UnityEngine;
using Meg.Networking;

namespace Meg.Parameters
{

    /** Represents a server string parameter - part of the Setup interface backend. */

    [System.Serializable]
    public class megParameterString : megParameter
    {

        // Properties
        // ------------------------------------------------------------

        /** The server data value to manipulate. */
        public string serverParam = "";

        /** Value to apply to server data. */
        public string serverValue
        {
            get { return serverUtils.GetServerDataAsText(serverParam); }
            set { serverUtils.PostServerData(serverParam, value);}
        }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        public megParameterString(megParameterGroup group = null) : base(megParameterType.String, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** String representation. */
        public override string ToString()
        {
            if (string.IsNullOrEmpty(serverParam))
                return base.ToString();

            var value = serverUtils.GetServerDataAsText(serverParam);
            return string.Format("{0}: {1} ({2})", serverParam, serverValue, value);
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
