using UnityEngine;
using System.Collections.Generic;

namespace Meg.EventSystem
{

    public class megEventRunner : AutoSingleton<megEventRunner>
    {

        // Members
        // ------------------------------------------------------------

        /** The set of registered event files. */
        private readonly List<megEventFile> _files = new List<megEventFile>();


        // Public Methods
        // ------------------------------------------------------------

        /** Start updating an event file. */
        public void AddFile(megEventFile file)
        {
            if (!_files.Contains(file))
                _files.Add(file);
        }

        /** Stop updating an event file. */
        public void RemoveFile(megEventFile file)
        {
            _files.Remove(file);
        }


        // Unity Methods
        // ------------------------------------------------------------

        /** Updating. */
        private void Update()
        {
            var n = _files.Count;
            for (var i = 0; i < n; i++)
                _files[i].Update(Time.time, Time.deltaTime);
        }

    }

}