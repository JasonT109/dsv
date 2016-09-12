using UnityEngine;
using System.Collections;
using Meg.Networking;

public class LatLongPoint : MonoBehaviour
{
    public Vector2 LatLong;

    public string latitudeServerParam;
    public string longitudeServerParam;

    private LatLongSpace _space;

	
	private void Update()
	{
	    if (!_space && transform.parent)
	        _space = transform.parent.GetComponent<LatLongSpace>();

        if (!_space)
            return;

	    if (!string.IsNullOrEmpty(latitudeServerParam))
            LatLong.y = serverUtils.GetServerData(latitudeServerParam, LatLong.y);
        if (!string.IsNullOrEmpty(longitudeServerParam))
            LatLong.x = serverUtils.GetServerData(longitudeServerParam, LatLong.x);

        transform.localPosition = _space.LatLongToLocal(LatLong);
	}
}
