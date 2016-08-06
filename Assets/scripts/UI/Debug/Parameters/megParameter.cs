using UnityEngine;
using System.Collections;
using System.Linq;

namespace Meg.Parameters
{

    /** Possible types of parameter. */
    public enum megParameterType
    {
        Value = 0
    }


    /** Base class for a shared state parameter. */
    [System.Serializable]
    public class megParameter
    {

        // Properties
        // ------------------------------------------------------------

        /** The type of event. */
        public readonly megParameterType type;

        /** File that this event belongs to. */
        public megParameterFile file { get { return _file; } }

        /** Whether event is selected. */
        public bool selected { get { return file != null && file.selectedParameter == this; } }

        /** Whether event is minimized. */
        public bool minimized { get { return !selected; } }

        /** Id for this event. */
        public virtual string name
            { get { return ToString(); } }


        // Members
        // ------------------------------------------------------------

        /** The file that this event belongs to. */
        private readonly megParameterFile _file;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for an event. */
        protected megParameter(megParameterType type, megParameterFile file = null)
        {
            this.type = type;
            _file = file;
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
        protected void PostServerData(string key, float value)
            { file.PostServerData(key, value); }

        /** Set a server value. */
        protected void PostServerData(string key, string value)
            { file.PostServerData(key, value); }

        /** Return a server value. */
        public float GetServerData(string key)
            { return file.GetServerData(key); }

    }

}
