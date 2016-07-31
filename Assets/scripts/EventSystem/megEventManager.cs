using UnityEngine;
using Meg.SonarEvent;

namespace Meg.EventSystem
{

    public class megEventManager : Singleton<megEventManager>
    {

        // Properties
        // ------------------------------------------------------------

        /** The sonar event manager. */
        public megSonarEventManager Sonar;


        // Unity Methods
        // ------------------------------------------------------------

        /** Initialization. */
        private void Awake()
        {
            // Ensure that the event manager instance is populated.
            EnsureInstanceExists();
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Start updating an event file. */
        public void AddFile(megEventFile file)
            { megEventRunner.Instance.AddFile(file); }

        /** Stop updating an event file. */
        public void RemoveFile(megEventFile file)
            { megEventRunner.Instance.RemoveFile(file); }


    }

}