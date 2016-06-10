using UnityEngine;
using System.Collections;

namespace Meg.EventSystem
{
    [System.Serializable]
    public class megEventGroup
    {
        public GameObject trigger;
        public float eventTime;
        public bool running;
        public megEventObject[] eventObjects;
    }

    [System.Serializable]
    public class megEventObject
    {
        public float triggerTime;
        public float completeTime;
        public bool running;
        public bool physicsEvent;
        public Vector3 physicsDirection;
        public float phyicsMagnitude;
        public string serverParam;
        public float serverValue;
        public bool completed;
    }

    [System.Serializable]
    public class megEventSonar
    {
        public GameObject trigger;
        public Vector3[] waypoints;
        public bool destroyOnEnd;
    }
}