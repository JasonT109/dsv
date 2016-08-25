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
        public megEventFile File { get { return _file; } }

        /** Whether a file is currently playing. */
        public bool Playing { get { return File != null && File.playing; } }


        // Members
        // ------------------------------------------------------------

        /** The sonar event manager. */
        private megSonarEventManager _sonar;

        /** The camera event manager. */
        private megMapCameraEventManager _mapCamera;

        /** The event file. */
        private readonly megEventFile _file = new megEventFile();


        // Unity Methods
        // ------------------------------------------------------------

        /** Initialization. */
        private void Awake()
        {
            // Ensure that the event manager instance is populated.
            EnsureInstanceExists();
        }

        /** Updating. */
        private void Update()
        {
            // Check if we have a current file.
            if (File == null)
                return;
            
            // Toggle playback with a keyboard shortcut.
            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftControl))
            {
                if (!File.running)
                    File.Start();
                else if (File.paused)
                    File.Resume();
                else
                    File.Pause();
            }

            // Rewind keyboard shortcut.
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Backspace)) && Input.GetKey(KeyCode.LeftControl))
            {
                if (File.canRewind)
                    File.Rewind();
            }
        }


        // Public Methods
        // ------------------------------------------------------------

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