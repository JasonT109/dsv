using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------------
// Copyright 2012-2015 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

[AddComponentMenu("AVPro Live Camera/IMGUI Display")]
public class AVProLiveCameraGUIDisplay : MonoBehaviour
{
	public AVProLiveCamera _liveCamera;

	public ScaleMode _scaleMode = ScaleMode.ScaleToFit;
	public Color _color = Color.white;
	public int  _depth = 0;
	
	public bool _fullScreen = true;
	public float _x = 0.0f;
	public float _y = 0.0f;
	public float _width = 1.0f;
	public float _height = 1.0f;

	public bool _flipX;
	public bool _flipY;

	//-------------------------------------------------------------------------
	
	public void OnGUI()
	{
		if (_liveCamera == null)
			return;

		_x = Mathf.Clamp01(_x);
		_y = Mathf.Clamp01(_y);
		_width = Mathf.Clamp01(_width);
		_height = Mathf.Clamp01(_height);

		if (_liveCamera.OutputTexture != null)
		{
			GUI.depth = _depth;
			GUI.color = _color;
			
			Rect rect;
			if (_fullScreen)
				rect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);
			else
				rect = new Rect(_x * (Screen.width-1), _y * (Screen.height-1), _width * Screen.width, _height * Screen.height);

			if (_flipX || _flipY)
			{
				Vector2 pivot = new Vector2(rect.x + (rect.width / 2), rect.y + (rect.height / 2));
				Vector2 scale = Vector2.one;
				if (_flipX)
					scale.x = -1.0f;
				if (_flipY)
					scale.y = -1.0f;
 				GUIUtility.ScaleAroundPivot (scale, pivot);
 			}

			GUI.DrawTexture(rect, _liveCamera.OutputTexture, _scaleMode, false);
		}
	}
}