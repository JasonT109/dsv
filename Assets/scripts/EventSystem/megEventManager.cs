using UnityEngine;
using Meg.SonarEvent;

namespace Meg.EventSystem
{

    public class megEventManager : Singleton<megEventManager>
    {

        // Properties
        // ------------------------------------------------------------

        /** The sonar event manager. */
        public megSonarEventManager Sonar { get { return GetSonarEventManager(); } }

        /** The camera event manager. */
        public megMapCameraEventManager MapCamera { get { return GetMapCameraEventManager(); } }

        /** The current event file (if any). */
        public megEventFile File { get; private set; }

        /** Whether a file is currently playing. */
        public bool Playing { get { return File != null && File.playing; } }


        // Members
        // ------------------------------------------------------------

        /** The sonar event manager. */
        private megSonarEventManager _sonar;

        /** The camera event manager. */
        private megMapCameraEventManager _mapCamera;


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
            if (!_sonar)
                _sonar = ObjectFinder.Find<megSonarEventManager>();

            return _sonar;
        }

        /** Look up the map camera event manager. */
        public megMapCameraEventManager GetMapCameraEventManager()
        {
            if (!_mapCamera)
                _mapCamera = megMapCameraEventManager.Instance;

            return _mapCamera;
        }

    }

}