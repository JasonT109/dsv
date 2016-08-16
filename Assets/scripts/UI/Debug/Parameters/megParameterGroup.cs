using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Meg.Parameters
{

    /** Represents a group of server parameters - part of the Setup interface backend. */

    [System.Serializable]
    public class megParameterGroup
    {

        // Properties
        // ------------------------------------------------------------

        /** Name of this parameter group. */
        public string id = "Group";

        /** Object used to trigger this group. */
        public GameObject trigger;

        /** The parameters managed by this group. */
        public List<megParameter> parameters = new List<megParameter>();

        /** The file that this group belongs to. */
        public megParameterFile file { get { return _file; } }

        /** Whether group is minimized. */
        public bool minimized { get { return _file == null || _file.selectedGroup != this; } }

        /** Whether group is empty. */
        public bool empty { get { return parameters.Count == 0; } }


        // Members
        // ------------------------------------------------------------

        /** The file that this group belongs to. */
        private readonly megParameterFile _file;


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor. */
        public megParameterGroup(megParameterFile file = null)
        {
            _file = file;
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Add an parameter of a given type. */
        public megParameter AddParameter(megParameterType type)
        {
            var e = CreateParameter(type);
            parameters.Add(e);
            return e;
        }

        /** Insert an parameter of a given type after the given parameter. */
        public megParameter InsertParameter(megParameterType type, megParameter insertAfter)
        {
            var e = CreateParameter(type);
            var insertIndex = parameters.IndexOf(insertAfter);
            if (insertIndex >= 0)
                parameters.Insert(insertIndex + 1, e);
            else
                parameters.Add(e);

            return e;
        }

        /** Create an parameter of a given type. */
        public megParameter CreateParameter(megParameterType type)
        {
            switch (type)
            {
                case megParameterType.Value:
                    return new megParameterValue(this);
                default:
                    return new megParameterValue(this);
            }
        }

        /** Remove an parameter from the group. */
        public void RemoveParameter(megParameter e)
        {
            parameters.Remove(e);

            if (e == file.selectedParameter)
                file.selectedParameter = null;
        }

        /** Clear parameters from group. */
        public void Clear()
        {
            parameters.Clear();

            if (file.selectedParameter != null && file.selectedParameter.group == this)
                file.selectedParameter = null;
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            var parametersJson = new JSONObject(JSONObject.Type.ARRAY);
            var parameters = this.parameters;
            foreach (var e in parameters)
                parametersJson.Add(e.Save());

            json.AddField("id", id);
            json.AddField("parameters", parametersJson);

            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            // Load in value parameters.
            json.GetField(ref id, "id");
      
            var parametersJson = json.GetField("parameters");
            parameters.Clear();
            for (var i = 0; i < parametersJson.Count; i++)
            {
                var s = parametersJson[i]["type"].str;
                var type = (megParameterType)Enum.Parse(typeof(megParameterType), s, true);
                var e = AddParameter(type);
                e.Load(parametersJson[i]);
            }
        }

    }

}
