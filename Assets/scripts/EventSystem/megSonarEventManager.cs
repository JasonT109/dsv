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
        public GameObject waypointVisual;
        public float visualXOffset = 36.295f;
        public float visualYOffset = 21.69f;
        public GameObject sonarObject;
        public float scale = 1f;
        public GameObject dsvObject;
        public GameObject playButton;
        public GameObject recordButton;
        public GameObject customButtonGroup;
        private buttonControl[] customButtons;

        private bool canPress = true;
        private Vector3[] wps;

        public void megSetVisualMarkers(megEventSonar m)
        {
            //get previously used markers and destroy them
            GameObject[] oldMarkers = GameObject.FindGameObjectsWithTag("Marker");
            foreach (GameObject g in oldMarkers)
            {
                Destroy(g);
            }
            if (m.waypoints.Length > 0)
            {
                for (int i = 0; i < m.waypoints.Length; i++)
                {
                    //spawn a visual waypoint
                    GameObject wp = (GameObject)Instantiate(waypointVisual, new Vector3(0, 0, 0), Quaternion.identity);
                    TextMesh t = wp.GetComponentInChildren<TextMesh>();

                    Debug.Log("Spawning marker");

                    t.text = i.ToString();
                    //parent to this object
                    wp.transform.parent = gameObject.transform;

                    //set the wp local position (x = x + xoffset, y = z, z = y + yoffset)
                    wp.transform.localPosition = new Vector3(m.waypoints[i].x + visualXOffset, m.waypoints[i].z, m.waypoints[i].y + visualYOffset);
                }
            }
        }

        void GetCustomButtons()
        {
            if (customButtonGroup)
            {
                //for (int i = 0; i < customButtonGroup.GetComponent<buttonGroup>().buttons.)
            }
        }

        public void megPlayMegSonarEvent(megEventSonar m)
        {
            Vector3[] wps = new Vector3[m.waypoints.Length];
            megSetVisualMarkers(m);

            for (int i = 0; i < m.waypoints.Length; i++)
            {
                //add sonar offset position to waypoints
                wps[i] = new Vector3(m.waypoints[i].x, m.waypoints[i].y + sonarYOffset, m.waypoints[i].z + sonarZOffset);

                //convert waypoints from local space to world space
                wps[i] = dsvObject.transform.TransformPoint(wps[i]);
            }

            //spawn the main sonar object
            GameObject so = (GameObject)Instantiate(sonarObject, wps[0], Quaternion.identity);

            //set the rotation so we are looking at second way point on spawn
            so.transform.rotation = Quaternion.LookRotation(wps[1] - wps[0]);

            //spawn it on the network
            NetworkServer.Spawn(so);

            //set the sonar objects waypoints
            so.GetComponent<graphicsSonarObject>().wayPoints = wps;
            so.GetComponent<graphicsSonarObject>().destroyOnLastPoint = m.destroyOnEnd;
        }

        void Start()
        {
            //get our sub so we know where to spawn the sonar object
            dsvObject = GameObject.FindWithTag("ServerData");

            //register the prefab so it can be spawned
            ClientScene.RegisterPrefab(sonarObject);
        }

        void Update()
        {
            for (int m = 0; m < sonarEvents.Length; m++)
            {
                var triggerScript = sonarEvents[m].trigger.GetComponent<buttonControl>();

                if (triggerScript.pressed && canPress)
                {
                    megPlayMegSonarEvent(sonarEvents[m]);
                    canPress = false;
                    StartCoroutine(wait(0.2f));
                }

            }
        }

        IEnumerator wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            canPress = true;
        }
    }
}
