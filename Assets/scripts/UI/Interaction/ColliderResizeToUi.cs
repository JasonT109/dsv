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

        var size = _rectTransform.sizeDelta;
        _collider.size = new Vector3(size.x, size.y, _collider.size.z);
    }

}
