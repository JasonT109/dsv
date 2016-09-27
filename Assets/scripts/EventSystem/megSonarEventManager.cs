using System.Linq;
using Meg.Networking;

namespace Meg.SonarEvent
{
    using UnityEngine;
    using UnityEngine.Networking;
    using System.Collections;
    using Meg.EventSystem;

    public class megSonarEventManager : NetworkBehaviour
    {

        public megEventSonar[] sonarEvents;
        public float sonarZOffset = 76.6f;
        public float sonarYOffset = -8.0f;
        public GameObject sonarObject;

        private bool sonarObjectRegistered;

        //use this as an anchor for the shark. In this case it's set as the Sonar's camera
        public GameObject Anchor { get; private set; }

        public void megPlayMegSonarEventByName(string name)
        {
            var sonarEvent = sonarEvents.First(e => e.eventName == name);
            if (sonarEvent != null)
                megPlayMegSonarEvent(sonarEvent);
        }

        public void megPlayMegSonarEvent(megEventSonar m)
        {
            if (!sonarObjectRegistered)
            {
                ClientScene.RegisterPrefab(sonarObject);
                sonarObjectRegistered = true;
            }

            // Make our best effort to locate the sonar root.
            if (!Anchor)
                Anchor = GameObject.FindWithTag("SonarRoot");
            if (!Anchor && SonarRoot.EnsureInstanceExists())
                Anchor = SonarRoot.Instance.gameObject;

            // Emit a warning if sonar root could not be found.
            if (!Anchor)
                Debug.LogWarning("Could not find Sonar root in scene! Add a SonarRoot behaviour to fix this.");

            var wps = new Vector3[m.waypoints.Length];

            for (int i = 0; i < m.waypoints.Length; i++)
            {
                //add sonar offset position to waypoints
                wps[i] = new Vector3(m.waypoints[i].x, m.waypoints[i].y + sonarYOffset, m.waypoints[i].z + sonarZOffset);
                wps[i] = new Vector3(Anchor.transform.TransformPoint(wps[i]).x, 
                    Anchor.transform.position.y + sonarYOffset, 
                    Anchor.transform.TransformPoint(wps[i]).z);
            }

            //spawn the main sonar object
            var so = (GameObject)Instantiate(sonarObject, wps[0], Quaternion.identity);

            //set the rotation so we are looking at second way point on spawn
            so.transform.rotation = Quaternion.LookRotation(wps[1] - wps[0]);

            //spawn it on the network
            NetworkServer.Spawn(so);

            //set the sonar objects waypoints
            so.GetComponent<graphicsSonarObject>().wayPoints = wps;
            so.GetComponent<graphicsSonarObject>().destroyOnLastPoint = m.destroyOnEnd;
        }

        public void megSonarClear()
        {
            var objects = FindObjectsOfType(typeof(graphicsSonarObject))
                .Cast<graphicsSonarObject>();

            foreach (var o in objects)
                Destroy(o.gameObject);
        }

        void Start()
        {
            if (!sonarObjectRegistered)
            {
                ClientScene.RegisterPrefab(sonarObject);
                sonarObjectRegistered = true;
            }
        }

    }
}
