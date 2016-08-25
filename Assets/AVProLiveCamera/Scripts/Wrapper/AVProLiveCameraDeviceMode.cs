using System.Text;

//-----------------------------------------------------------------------------
// Copyright 2012-2015 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

public class AVProLiveCameraDeviceMode
{
	private AVProLiveCameraDevice _device;
	private int _modeIndex;
	private int _width, _height;
	private float _fps;
	private string _format;
	
	public int Width
	{
		get { return _width; }
	}
	
	public int Height
	{
		get { return _height; }
	}

	public float FPS
	{
		get { return _fps; }
	}

	public string Format
	{
		get { return _format; }
	}
	
	public int Index
	{
		get { return _modeIndex; }
	}	
	
	public AVProLiveCameraDevice Device
	{
		get { return _device; }
	}

	public AVProLiveCameraDeviceMode(AVProLiveCameraDevice device, int index, int width, int height, float fps, string format)
	{
		_device = device;
		_modeIndex = index;
		_width = width;
		_height = height;
		_fps = fps;
		_format = format;
	}
}