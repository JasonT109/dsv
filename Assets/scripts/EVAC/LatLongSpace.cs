using UnityEngine;
using System.Collections;
using Meg.Maths;

/**
 * Provides a space that is registered to latitude and longitude via 
 * two markers.  Child objects with LatLongPoints on them will be 
 * automatically positioned according to their LatLong coordinate.
 */

public class LatLongSpace : MonoBehaviour
{

    public LatLongMarker Marker1;
    public LatLongMarker Marker2;

    public Vector3 LatLongToLocal(Vector2 latLon)
    {
        var x = graphicsMaths.remapValue(latLon.x, 
            Marker1.LatLong.x, 
            Marker2.LatLong.x, 
            Marker1.transform.localPosition.x,
            Marker2.transform.localPosition.x);

        var y = graphicsMaths.remapValue(latLon.y, 
            Marker1.LatLong.y, 
            Marker2.LatLong.y,
            Marker1.transform.localPosition.y, 
            Marker2.transform.localPosition.y);

        return new Vector3(x, y, 0);
    }

    public Vector2 LocalToLatLong(Vector3 p)
    {
        var x = graphicsMaths.remapValue(p.x,
            Marker1.transform.localPosition.x, 
            Marker2.transform.localPosition.x,
            Marker1.LatLong.x, 
            Marker2.LatLong.x);

        var y = graphicsMaths.remapValue(p.y,
            Marker1.transform.localPosition.y, 
            Marker2.transform.localPosition.y,
            Marker1.LatLong.y, 
            Marker2.LatLong.y);

        return new Vector2(x, y);
    }
}
