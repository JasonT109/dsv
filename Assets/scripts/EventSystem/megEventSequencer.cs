using UnityEngine;
using System.Collections;
using Meg.EventSystem;
using Meg.Networking;

public class megEventSequencer : MonoBehaviour {

    public megEventGroup megEvents = new megEventGroup();
    private megEventObject[] megEventList;
    private float eventGroupTimer;
    private float[] eventObjectTimers;
    private float[] initParams;
    private float[] initialValues; //used for when we need to lerp over time

    // Use this for initialization
    void Start ()
    {
        megEventList = new megEventObject[megEvents.eventObjects.Length];
        initParams = new float[megEvents.eventObjects.Length];
        eventObjectTimers = new float[megEvents.eventObjects.Length];
        initialValues = new float[megEvents.eventObjects.Length];
        for (int i = 0; i < megEvents.eventObjects.Length; i++)
        {
            initialValues[i] = -1337f;
            megEventList[i] = megEvents.eventObjects[i];
            initParams[i] = serverUtils.GetServerData(megEvents.eventObjects[i].serverParam);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if trigger is active
        if (megEvents.trigger.GetComponent<buttonControl>().active && !megEvents.running)
        {
            megEvents.running = true;
        }

        //if event group is running
        if (megEvents.running)
        {
            //increment time
            megEvents.eventTime += Time.deltaTime;

            //for each event
            for (int i = 0; i < megEventList.Length; i++)
            {
                //if event trigger time less than group time and event is not running
                if(megEventList[i].triggerTime < megEvents.eventTime && !megEventList[i].running)
                {
                    megEventList[i].running = true;
                }

                //if event is RUNNING and TRIGGER time < EVENT time and has NOT COMPLETED
                if (megEventList[i].running && megEventList[i].triggerTime < megEvents.eventTime && !megEventList[i].completed)
                {
                    eventObjectTimers[i] += Time.deltaTime;
                    if (megEventList[i].completeTime == 0 || megEventList[i].physicsEvent)
                    {
                        if (megEventList[i].physicsEvent)
                        {
                            Debug.Log("Physics event.");
                            gameObject.GetComponent<serverData>().RpcImpact(megEventList[i].physicsDirection * megEventList[i].phyicsMagnitude);
                            megEventList[i].completed = true;
                        }
                        else
                        {
                            //set serverParam
                            //Debug.Log("Triggering event now!");
                            gameObject.GetComponent<serverData>().OnValueChanged(megEventList[i].serverParam, megEventList[i].serverValue);
                            //set to complete
                            megEventList[i].completed = true;
                        }
                    }
                    else
                    {
                        //blend serverParam over time
                        //Debug.Log("Started blend event over " + megEventList[i].completeTime + " seconds.");

                        //get the value from the server
                        if (initialValues[i] == -1337f)
                        {
                            initialValues[i] = serverUtils.GetServerData(megEventList[i].serverParam);
                        }

                        //lerp it over time
                        float initialValue = initialValues[i];
                        initialValue = Mathf.Lerp(initialValues[i], megEventList[i].serverValue, eventObjectTimers[i] / megEventList[i].completeTime);

                        //set the value on the server
                        gameObject.GetComponent<serverData>().OnValueChanged(megEventList[i].serverParam, initialValue);

                        //set to complete when completeTime is up
                        if (megEventList[i].completeTime + megEventList[i].triggerTime < megEvents.eventTime)
                        {
                            megEventList[i].completed = true;
                        }
                    }
                }
            }
        }
        if (megEvents.running && !megEvents.trigger.GetComponent<buttonControl>().active)
        {
            megEvents.running = false;
            megEvents.eventTime = 0;
            //reset each event
            for (int i = 0; i < megEventList.Length; i++)
            {
                //reset the parameter we changed
                gameObject.GetComponent<serverData>().OnValueChanged(megEventList[i].serverParam, initParams[i]);

                //set running false
                megEventList[i].running = false;

                //reset completed status
                megEventList[i].completed = false;

                //reset to a nonsense number
                initialValues[i] = -1337f;

                //reset individual timers
                eventObjectTimers[i] = 0;
            }
        }
    }
}
