namespace Meg.SonarEvent
{
    using UnityEngine;
    using UnityEngine.Networking;
    using System.Collections;
    using Meg.EventSystem;

    public class megSonarEventManager : NetworkBehaviour
    {

        public megEventSonar[] sonarEvents;
        //public float sonarXOffset = 10.0f;
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

        //use this as an anchor for the shark. In this case it's set as the Sonar's camera
        public GameObject Anchor;

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
                //wps[i] = new Vector3(m.waypoints[i].x, 10.0f, m.waypoints[i].z + sonarZOffset);

                //rotate based on sonar anchor's angle
                //Vector2 P1;
                //P1.x = x;
                //P1.y = z;
                //
                //Vector2 P2;
                //P2.x = Anchor.transform.position.x + (x/4f);
                //P2.y = Anchor.transform.position.z + (z/4f);
                //
                //Vector2 P3 = rotateAboutPoint2D(P1,P2, Anchor.transform.rotation.eulerAngles.y);

                //float x = ((Anchor.transform.position.x + 40f)/80f) * 400f - 200f;
                //float z = ((Anchor.transform.position.z + 40f)/80f) * 140f - 70f;

                //normalise waypoint data
                float x = (wps[i].x + 40)/80;
                float z = (wps[i].z + -60)/30;

                //convert to sonarscale
                //x = x * 400 - 200;
                //z = z * 140 - 70;

                x = x * 110f - 55f;
                z = z * 35f - 27.5f;

                Vector2 PreRotate = new Vector2(dsvObject.transform.position.x + x, dsvObject.transform.position.z + z + 83f);

                Vector2 PostRotate = rotateAboutPoint2D(PreRotate, new Vector2(dsvObject.transform.position.x, dsvObject.transform.position.z), -(dsvObject.transform.rotation.eulerAngles.y * 0.0174533f));

                wps[i] = new Vector3(PostRotate.x, Anchor.transform.position.y + sonarYOffset -30.0f, PostRotate.y);


                //convert waypoints from local space to world space
                //wps[i] = dsvObject.transform.TransformPoint(wps[i]);
                //wps[i] = new Vector3(dsvObject.transform.TransformPoint(wps[i]).x, Anchor.transform.position.y + sonarYOffset -30.0f, dsvObject.transform.TransformPoint(wps[i]).z);
                //wps[i] = new Vector3(dsvObject.transform.TransformPoint(wps[i]).x, Anchor.transform.position.y + sonarYOffset -30.0f, dsvObject.transform.TransformPoint(wps[i]).z);

                //wps[i] = new Vector3(Anchor.transform.localPosition.x + (x/4f), Anchor.transform.position.y + sonarYOffset -30.0f, Anchor.transform.localPosition.z + (z/4f));

               // wps[i] = new Vector3(dsvObject.transform.TransformPoint(wps[i]).x, 
               //     Anchor.transform.position.y + sonarYOffset -30.0f, 
               //     dsvObject.transform.TransformPoint(wps[i]).z - 35f * (Mathf.Sin(dsvObject.transform.rotation.eulerAngles.x * 0.0174533f)));

                //wps[i] = new Vector3(P3.x, Anchor.transform.position.y + sonarYOffset -30.0f, P3.y);
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

        Vector2 rotateAboutPoint2D(Vector2 _p1, Vector2 _p2, float _angle)
        {
            //rotate about a point in 2D
            //rotates _p1 about _p2
            //Vector2 r;
            //r.x = Mathf.Cos(_angle) * (_p2.x - _p1.x) - Mathf.Sin(_angle) * (_p2.y - _p1.y) + _p1.x;
            //r.y = Mathf.Sin(_angle) * (_p2.x - _p1.x) + Mathf.Cos(_angle) * (_p2.y - _p1.y) + _p1.y;
            //
            ////
            //return (r);



            float s = Mathf.Sin(_angle);
            float c = Mathf.Cos(_angle);

            // translate point back to origin:
            _p1.x -= _p2.x;
            _p1.y -= _p2.y;

            // rotate point
            float xnew = _p1.x * c - _p1.y * s;
            float ynew = _p1.x * s + _p1.y * c;

            // translate point back:
            _p1.x = xnew + _p2.x;
            _p1.y = ynew + _p2.y;
            return _p1;
        }
    }
}
