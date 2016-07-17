using UnityEngine;
using System.Collections;
using Meg.Networking;
using Vectrosity;


public class NavSubPin : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    /** Vessel that this pin represents. */
    public int VesselId;

    /** Visual indicator for height above ocean floor. */
    public GameObject vesselHeightIndicator;

    /** Pressable button for this pin. */
    public GameObject vesselButton;

    /** 3D representation of the vessel. */
    public GameObject vesselModel;

    /** Intercept line color. */
    public Color InterceptLineColor;

    /** Distance to ocean floor. */
    public float Distance;


    // Private Properties
    // ------------------------------------------------------------

    private float LineXOffset
    { get { return _manager.lineXOffset; } }

    private Vector2 ImageSize
    { get { return _manager.imageSize; } }

    
    // Members
    // ------------------------------------------------------------

    /** The pin manager. */
    private NavSubPins _manager;

    /** Raycast result, used to locate ocean floor. */
    private RaycastHit _hit;

    /** Interception line. */
    private VectorLine _interceptLine;


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        _manager = GetComponentInParent<NavSubPins>();
    }

    /** Enabling. */
    private void OnEnable()
    {
        _interceptLine = VectorLine.SetLine(InterceptLineColor, new Vector3[2]);
        _interceptLine.lineWidth = 2;
    }

    /** Disabling. */
    private void OnDisable()
    {
        VectorLine.Destroy(ref _interceptLine);
    }


    // Public Methods
    // ------------------------------------------------------------

    /** Updating. */
    public void UpdatePin()
    {
        // Update pin visibility.
        var visible = serverUtils.GetVesselVis(VesselId);
        vesselButton.SetActive(visible);
        vesselHeightIndicator.SetActive(visible);

        // Get vessel's server position and apply that to the vessel model.
        Vector3 position = new Vector3();
        float velocity;
        serverUtils.GetVesselData(VesselId, out position, out velocity);
        vesselModel.transform.localPosition = ConvertVesselCoords(position);

        // Get position in map space and position button there.
        var mapPos = ConvertToMapSpace(vesselModel.transform.position);
        vesselButton.transform.localPosition = mapPos;

        // Cast a ray down to the terrain from the original position.
        if (Physics.Raycast(vesselModel.transform.position, -Vector3.up, out _hit))
            Distance = _hit.distance;

        // Update height indicator.
        if (Distance > 0)
        {
            // Set the position of the height indicators to be at ground level
            var groundPos = ConvertToMapSpace(_hit.point);
            vesselHeightIndicator.transform.localPosition = groundPos;

            // Set the x position to be exactly the same as button plus offset
            vesselHeightIndicator.transform.localPosition =
                new Vector3(mapPos.x + LineXOffset,
                    vesselHeightIndicator.transform.localPosition.y,
                    vesselHeightIndicator.transform.localPosition.z + 1);
        }

        // Update the height indicator's length.
        float vesselHeight = mapPos.y - vesselHeightIndicator.transform.localPosition.y;
        vesselHeightIndicator.GetComponent<graphicsSlicedMesh>().Height = vesselHeight;
        vesselHeightIndicator.GetComponent<Renderer>()
            .material.SetTextureScale("_MainTex", new Vector2(1, 4 * vesselHeight));

        // Update map icon with a direction indicator.
        UpdateMapIconDirection();
        
    }

    /** Update the interception indicator for this vessel (if intercepting). */
    public void UpdateInterceptIndicator()
    {
        // Check if this vessel is performing an interception.
        var intercept = serverUtils.GetVesselMovements().GetVesselMovement(VesselId) as vesselIntercept;
        _interceptLine.active = intercept != null;
        if (intercept == null)
            return;

        // Locate interception pin.
        var interceptPin = _manager.GetVesselPin(intercept.TargetVessel);

        // Get interception locations.
        var from = vesselButton.transform.position;
        var to = interceptPin.vesselButton.transform.position;

        // Update interception indicator.
        _interceptLine.points3[0] = new Vector3(from.x, from.y, from.z + 2);
        _interceptLine.points3[1] = new Vector3(to.x, to.y, to.z + 2);
        _interceptLine.Draw3D();

    }


    // Private Methods
    // ------------------------------------------------------------

    private void UpdateMapIconDirection()
    {
        // 0 no direction, 1 left, 2 upleft, 3 up, 4 up right, 5 right, 6 down right, 7 down, 8 down left
        int mapIconDirection = 0;
        var local = vesselButton.transform.localPosition;
        var x = ImageSize.x;
        var y = ImageSize.y;

        // Check to see if child is at edge of the map.
        if (local.x == -x && local.y == -y)
            mapIconDirection = 8;
        else if (local.x == -x && local.y == y)
            mapIconDirection = 2;
        else if (local.x == -x)
            mapIconDirection = 1;

        if (local.x == x && local.y == -y)
            mapIconDirection = 6;
        else if (local.x == x && local.y == y)
            mapIconDirection = 4;
        else if (local.x == x)
            mapIconDirection = 5;

        if (local.y == y && local.x != x && local.x != -x)
            mapIconDirection = 3;
        if (local.y == -y && local.x != x && local.x != -x)
            mapIconDirection = 7;

        // Set the orientation of the child to indicate the direction.
        vesselButton.GetComponent<graphicsMapIcon>().atBounds = mapIconDirection != 0;
        vesselButton.GetComponent<graphicsMapIcon>().direction = mapIconDirection;
    }

    /** Convert a vessel's position into 2D map space. */
    private Vector2 ConvertToMapSpace(Vector3 p)
        { return _manager.ConvertToMapSpace(p); }

    /** Convert a vessel's position into 3D map space. */
    private Vector3 ConvertVesselCoords(Vector3 p)
        { return _manager.ConvertVesselCoords(p); }


}
