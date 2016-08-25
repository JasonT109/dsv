/// Copyright 2015 Jetro Lauha (Strobotnik Ltd)

using UnityEngine;
using UnityEditor;

public class DynamicTextDetectSceneSave : UnityEditor.AssetModificationProcessor
{
    public static string[] OnWillSaveAssets(string[] paths)
    {
        // Saving scene or project needs mesh regeneration for all DT objects.
        // (Fixes regression on Unity v5.0+ which seems clear font atlases on
        //  all save operations, without texture rebuild callback).

        //Debug.Log("OnWillSaveAssets, " + paths.Length + " paths");

        DynamicTextEditorExtensions.needToRegenerateMeshes = true;
        return paths;
    }
}
