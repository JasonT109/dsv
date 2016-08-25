/// Copyright 2013-2015 Jetro Lauha (Strobotnik Ltd)

using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class DynamicTextEditorExtensions
{
    public static bool needToRegenerateMeshes = false;

    [UnityEditor.MenuItem("GameObject/Create Other/Dynamic Text")]
    static void createDynamicTextGameObject()
    {
        //Debug.Log("DynamicText: (GameObject/Create Other/Dynamic Text)");
        GameObject go = new GameObject("Dynamic Text");

        // enable these rows if you want new one to be child of selection
        //if (Selection.activeTransform != null)
        //    go.transform.parent = Selection.activeTransform;

        DynamicText dt = go.AddComponent<DynamicText>();
        dt.SetText("Text");

        if (dt.cam)
        {
            float camDistance = 10.0f;
            go.transform.position = dt.cam.transform.position + dt.cam.transform.forward * camDistance;

            // This may be helpful for multi-camera scenes - copy layer from
            // camera. Feel free to disable this row if you don't want it:
            dt.gameObject.layer = dt.cam.gameObject.layer;
        }
    }

    [UnityEditor.MenuItem("CONTEXT/TextMesh/Convert to Dynamic Text")]
    static void convertTextMeshToDynamicText(MenuCommand command)
    {
        //Debug.Log("DynamicText: (CONTEXT/TextMesh/Convert to Dynamic Text)");
        TextMesh tm = command.context as TextMesh;
        if (tm != null && tm.gameObject)
        {
            GameObject go = tm.gameObject;

            Undo.AddComponent<DynamicText>(go);
        }
    }


    static void dt_editorUpdate()
    {
        if (needToRegenerateMeshes)
        {
            //Debug.Log("needToRegenerateMeshes was true; " + Time.realtimeSinceStartup);
            DynamicText[] dts = GameObject.FindObjectsOfType<DynamicText>();
            foreach (DynamicText dt in dts)
            {
                dt.GenerateMesh();
            }
            needToRegenerateMeshes = false;
        }
    }

    static DynamicTextEditorExtensions()
    {
        // This version of Dynamic Text only supports Unity v5.0+
        // Use Unity 4.x to download from Asset Store a version for Unity v4.2-4.6

        #if UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6
        Debug.LogError("This version of DynamicText doesn't support Unity 4.x!");
        #endif

        // Cleanup hack used with earlier version(s)
        PlayerPrefs.DeleteKey("DynamicText_force_alternate_sampling");

        EditorApplication.update -= dt_editorUpdate;
        EditorApplication.update += dt_editorUpdate;
    }
}
