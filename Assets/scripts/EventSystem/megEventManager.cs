using UnityEngine;
using Meg.SonarEvent;

namespace Meg.EventSystem
{

    public class megEventManager : Singleton<megEventManager>
    {

        // Properties
        // ------------------------------------------------------------

        /** The sonar event manager. */
        public megSonarEventManager Sonar { get; private set; }

        /** The camera event manager. */
        public megMapCameraEventManager MapCamera { get; private set; }

        /** The current event file (if any). */
        public megEventFile File { get; private set; }

        /** Whether a file is currently playing. */
        public bool Playing { get { return File != null && File.playing; } }


        // Unity Methods
        // ------------------------------------------------------------

        /** Initialization. */
        private void Awake()
        {
            // Ensure that the event manager instance is populated.
            EnsureInstanceExists();

            // Resolve links to other managers.
            if (!Sonar)
                Sonar = ObjectFinder.Find<megSonarEventManager>();
            if (!MapCamera)
                MapCamera = ObjectFinder.Find<megMapCameraEventManager>();
        }


        // Public Methods
        // ------------------------------------------------------------

        /** Set an event file as the current one. */
        public void SetCurrentFile(megEventFile file)
            { File = file; }

        /** Start updating an event file. */
        public void StartUpdating(megEventFile file)
            { megEventRunner.Instance.AddFile(file); }

        /** Stop updating an event file. */
        public void StopUpdating(megEventFile file)
            { megEventRunner.Instance.RemoveFile(file); }

        /** Look up the sonar event manager. */
        public megSonarEventManager GetSonarEventManager()
        {
            return Sonar;
        }

        /** Look up the map camera event manager. */
        public megMapCameraEventManager GetMapCameraEventManager()
        {
            if (MapCamera)
                return MapCamera;

            MapCamera = megMapCameraEventManager.Instance;

            return MapCamera;
        }

    }

}