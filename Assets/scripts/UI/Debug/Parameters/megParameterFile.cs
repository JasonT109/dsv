using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Meg.Networking;

namespace Meg.Parameters
{

    /** 
     * A file containing a collection of editable parameters.  The File is organized 
     * into named Groups, each containing a list of Parameters.  A Parameter represents any
     * editable aspect of the simulation, but is currently limited to editing server 
     * parameter values.
     * 
     * This is the primary backing data structure for the debug Setup screen, where users
     * can load in and edit lists of server parameters in order to set the simulation into
     * a desired state.
     * 
     * The interface logic for editing parameter files can be found in debugParameterFileUi.
     * 
     */

    [System.Serializable]
    public class megParameterFile
    {

        // Properties
        // ------------------------------------------------------------

        /** The parameters managed by this file. */
        public List<megParameterGroup> groups = new List<megParameterGroup>();

        /** Whether file is empty. */
        public bool empty { get { return groups.Count == 0; } }

        /** Whether file can be added to. */
        public bool canAdd { get { return true; } }

        /** Whether file can be removed from. */
        public bool canRemove { get { return !empty; } }

        /** Whether file can be cleared. */
        public bool canClear { get { return !empty; } }

        /** Whether file can be saved at the moment. */
        public bool canSave { get { return !empty; } }

        /** The selected parameter group (if any). */
        public megParameterGroup selectedGroup { get; set; }

        /** The selected parameter (if any). */
        public megParameter selectedParameter { get; set; }


        // Parameters
        // ------------------------------------------------------------

        /** General signature for file parameters. */
        public delegate void megParameterFileHandler(megParameterFile file);

        /** Parameter fired when file is loaded. */
        public megParameterFileHandler Loaded;

        /** Parameter fired when file is saved. */
        public megParameterFileHandler SavedToFile;

        /** Parameter fired when file is cleared. */
        public megParameterFileHandler Cleared;


        // Structures
        // ------------------------------------------------------------

        /** Tracking data for an applied parameter. */
        private struct ParameterRecord<T>
        {
            public float time;
            public T value;
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Add an group of a given type. */
        public megParameterGroup AddGroup()
        {
            var group = CreateGroup();
            groups.Add(group);
            return group;
        }

        /** Insert an group of a given type after the given group. */
        public megParameterGroup InsertGroup(megParameterGroup insertAfter)
        {
            var group = CreateGroup();
            var insertIndex = groups.IndexOf(insertAfter);
            if (insertIndex >= 0)
                groups.Insert(insertIndex + 1, group);
            else
                groups.Add(group);

            return group;
        }

        /** Create an group of a given type. */
        public megParameterGroup CreateGroup()
        { return new megParameterGroup(this); }

        /** Remove an group from the group. */
        public void RemoveGroup(megParameterGroup group)
        {
            groups.Remove(group);

            if (group == selectedGroup)
                selectedGroup = null;
        }

        /** Clear the file of all groups. */
        public void Clear()
        {
            groups.Clear();

            selectedGroup = null;
            selectedParameter = null;

            if (Cleared != null)
                Cleared(this);
        }

        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public virtual JSONObject Save()
        {
            var json = new JSONObject();
            var parametersJson = new JSONObject(JSONObject.Type.ARRAY);
            foreach (var g in groups)
                parametersJson.Add(g.Save());

            json.AddField("groups", parametersJson);
            return json;
        }

        /** Load state from JSON. */
        public virtual void Load(JSONObject json)
        {
            // Load in value parameters.
            var parametersJson = json.GetField("groups");
            for (var i = 0; i < parametersJson.Count; i++)
            {
                var group = AddGroup();
                group.Load(parametersJson[i]);
            }

            if (Loaded != null)
                Loaded(this);
        }

        /** Load state from a JSON file. */
        public virtual void LoadFromFile(string path)
        {
            var text = File.ReadAllText(path);
            var json = new JSONObject(text);
            Load(json);
        }

        /** Save state to a JSON file. */
        public virtual void SaveToFile(string path)
        {
            var json = Save();
            var text = json.Print(true);

            var info = new FileInfo(path);
            var folder = info.DirectoryName;
            if (folder != null && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText(path, text);

            if (SavedToFile != null)
                SavedToFile(this);
        }

    }
}
