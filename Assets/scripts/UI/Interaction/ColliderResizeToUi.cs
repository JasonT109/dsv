using UnityEngine;
using System.Collections;

public class ColliderResizeToUi : MonoBehaviour
{

    public RectTransform Transform;
    public BoxCollider Collider;

	private void Start()
	{
        if (!Transform)
	        Transform = GetComponent<RectTransform>();

        if (!Collider)
            Collider = GetComponent<BoxCollider>();

	    Update();
	}
	
	private void Update()
    {
        if (!Collider || !Transform)
            return;

        var rect = Transform.rect;
        Collider.size = new Vector3(rect.width, rect.height, Collider.size.z);
    }

}
