using UnityEngine;
using UnityEditor;
using System.Collections;

//-----------------------------------------------------------------------------
// Copyright 2012-2014 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

[CustomEditor(typeof(AVProLiveCameraManager))]
public class AVProLiveCameraManagerEditor : Editor
{
	private AVProLiveCameraManager _manager;

	public override void OnInspectorGUI()
	{
		_manager = (this.target) as AVProLiveCameraManager;
		
		if (!Application.isPlaying)
		{
			DrawDefaultInspector();
		}
		else
		{
			EditorGUILayout.Space();
			
			int numDevices = _manager.NumDevices;
			EditorGUILayout.PrefixLabel("Devices: ");
			for (int deviceIndex = 0; deviceIndex < numDevices; deviceIndex++)
			{
				EditorGUILayout.BeginHorizontal();
				AVProLiveCameraDevice device = _manager.GetDevice(deviceIndex);
				EditorGUILayout.LabelField(deviceIndex.ToString() + ") " + device.Name, "");
				if (device.IsRunning)
					EditorGUILayout.LabelField("Display at " + device.DisplayFPS.ToString("F1") + " FPS", "");
				else
					EditorGUILayout.LabelField("Stopped", "");
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}
	}
}