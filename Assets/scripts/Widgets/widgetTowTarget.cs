using UnityEngine;
using System.Collections;
using Meg.Networking;
using Meg.Maths;

public class widgetTowTarget : MonoBehaviour
{

    [Header ("Target graphic")]
    public GameObject towingTarget;

    [Header ("Max movement distance on x")]
    public float AmountX = 0.0f;
    public float MaxX = 10f;

    [Header("Max movement distance on y")]
    public float AmountY = 0.0f;
    public float MaxY = 10f;

    [Header("Baseline time between updates")]
    public float BaselineUpdateTick = 0.1f;

    [Header("Range added to baseline")]
    public float UpdateTickMin = 0.2f;
    public float UpdateTickMax = 1f;

    [Header("Drifting behaviour")]
    public float CloseDistance = 1f;
    public float TurnSpeed = 2.0f;
    public float PositionalDriftSpeed = 1f;

    private float _moveSpeed = 1.0f;
    private float _drift = 0.01f;
    private Vector3 _angle;
    private float _timeCheck;
    private Vector3 _newPos;
    private Vector3 _originalPos;
    private Vector3 _directionVector;

    void GetServerTowData()
    {
        _originalPos = new Vector3(serverUtils.GetServerData("towtargetx"), serverUtils.GetServerData("towtargety"), transform.position.z);

        //remap x to -40 to 40
        _originalPos.x = graphicsMaths.remapValue(_originalPos.x, 0, 1, -40, 40);

        //remap y to -20 to 20
        _originalPos.y = graphicsMaths.remapValue(_originalPos.y, 0, 1, -20, 20);

        _moveSpeed = serverUtils.GetServerData("towtargetspeed");
        _drift = _moveSpeed * 0.6f;
    }

    void Start ()
    {
        GetServerTowData();
    }

    // Update is called once per frame
    void Update ()
    {
        if (serverUtils.GetServerData("towtargetvisible") == 1)
        {
            Vector3 _oldPos = _newPos;
            if (Time.time > _timeCheck)
            {
                GetServerTowData();

                //create a new position relative to where we are
                _newPos = new Vector3(_originalPos.x + Random.Range(-AmountX, AmountX), _originalPos.y + Random.Range(-AmountY, AmountY), _originalPos.z);
                _newPos = new Vector3(Mathf.Clamp(_newPos.x, _originalPos.x - MaxX, _originalPos.x + MaxX), Mathf.Clamp(_newPos.y, _originalPos.y - MaxY, _originalPos.y + MaxY), _originalPos.z);

                //determine where the next point is in relation to object
                _directionVector = (_newPos - transform.position).normalized;
                _angle = new Vector3(0, 0, Vector3.Angle(transform.forward, _directionVector));
                _angle.z = _angle.z / 180;

                //add time
                _timeCheck = Time.time + (BaselineUpdateTick + Random.Range(UpdateTickMin, UpdateTickMax));
            }

            //reduce angle to 0
            _angle.z = Mathf.Lerp(_angle.z, 0, Time.deltaTime);

            //lerp position by positional drift
            _newPos = Vector3.Lerp(_newPos, _oldPos, Time.deltaTime * PositionalDriftSpeed);

            //lerp to new position
            transform.position = Vector3.Lerp(transform.position + (transform.forward * _angle.z * _drift), _newPos, Time.deltaTime * _moveSpeed);

            //xy plane only movement
            transform.position = new Vector3(transform.position.x, transform.position.y, _originalPos.z);

            //check we are far enough away to still turn to our point, this will prevent swivelling on the spot
            if (Vector3.Distance(_newPos, transform.position) > CloseDistance)
            {
                //rotate to angle
                Vector3 p = Vector3.RotateTowards(transform.forward, (_newPos - transform.position).normalized, Time.deltaTime * TurnSpeed, 1.0f);
                transform.rotation = Quaternion.LookRotation(p);
            }
        }
    }
}
