using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

namespace Meg.Parameters
{

    /** Possible types of parameter. */
    public enum megParameterType
    {
        Value = 0
    }


    /** Base class for a shared state parameter - backing data for the Setup interface. */

    [System.Serializable]
    public class megParameter
    {

        // Properties
        // ------------------------------------------------------------

        /** The type of parameter. */
        public readonly megParameterType type;

        /** Group that this parameter belongs to. */
        public megParameterFile file { get { return _group.file; } }

        /** Group that this parameter belongs to. */
        public megParameterGroup group { get { return _group; } }

        /** Whether parameter is selected. */
        public bool selected { get { return group != null && file.selectedParameter == this; } }

        /** Whether parameter is minimized. */
        public bool minimized { get { return !selected; } }

        /** Id for this parameter. */
        public virtual string name
            { get { return ToString(); } }


        // Members
        // ------------------------------------------------------------

        /** The group that this parameter belongs to. */
        private readonly megParameterGroup _group;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an parameter. */
        protected megParameter(megParameterType type, megParameterGroup group = null)
        {
            this.type = type;
            _group = group;
        }


        // Public Methods
        // ------------------------------------------------------------

        /** String representation. */
        public override string ToString()
        {
            return type.ToString();
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            json.AddField("type", type.ToString());
            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
        }


        // Server values
        // ------------------------------------------------------------

        /** Set a server value. */
        protected void PostServerData(string key, float value, bool add)
            { serverUtils.PostServerData(key, value, add); }

        /** Set a server value. */
        protected void PostServerData(string key, string value)
            { serverUtils.PostServerData(key, value); }

        /** Return a server value. */
        public float GetServerData(string key)
            { return serverUtils.GetServerData(key); }

    }

}
