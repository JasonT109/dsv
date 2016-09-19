using UnityEngine;
using System.Collections;

public class ColliderResizeToUi : MonoBehaviour
{

    private RectTransform _rectTransform;
    private BoxCollider _collider;

	private void Start()
	{
	    _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<BoxCollider>();

	    Update();
	}
	
	private void Update()
    {
        if (!_collider || !_rectTransform)
            return;

        var rect = _rectTransform.rect;
        _collider.size = new Vector3(rect.width, rect.height, _collider.size.z);
    }

}
