/// Copyright 2014,2015 Jetro Lauha (Strobotnik Ltd)

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DynamicText)), CanEditMultipleObjects]
public class DynamicTextInspector : Editor
{
    public void OnEnable()
    {
        //Debug.Log("DynamicTextInspector.OnEnable");
        Undo.undoRedoPerformed += UndoRedoPerformed;
    }

    public void OnDisable()
    {
        //Debug.Log("DynamicTextInspector.OnDisable");
        Undo.undoRedoPerformed -= UndoRedoPerformed;
    }

    private void UndoRedoPerformed()
    {
        if (target == null)
            return; // object has been destroyed

        serializedObject.Update();
        if (serializedObject.isEditingMultipleObjects)
        {
            //Debug.Log("UndoRedoPerformed - targets, count: " + targets.Length);
            foreach (Object o in targets)
            {
                DynamicText dt = o as DynamicText;
                dt.SetText(dt.serializedText);
            }
        }
        else
        {
            //Debug.Log("UndoRedoPerformed - target: " + target + " " + (target as DynamicText).serializedText);
            (target as DynamicText).SetText((target as DynamicText).serializedText);
        }
    }

    public override void OnInspectorGUI()
    {
        Color defaultColor = GUI.color;
        Color warningColor = new Color(1, 0.25f, 0.25f, 1);
        DynamicText dt = (DynamicText)target;

        serializedObject.Update();

        bool prefabs = false;
        if (!serializedObject.isEditingMultipleObjects)
        {
            prefabs = (PrefabUtility.GetPrefabType(dt) == PrefabType.Prefab);
        }
        else
        {
            foreach (DynamicText t in targets)
            {
                if (PrefabUtility.GetPrefabType(t) == PrefabType.Prefab)
                    prefabs = true;
            }
        }

        if (prefabs)
        {
            EditorGUILayout.HelpBox("(No camera reference in prefabs)", MessageType.Info);
        }
        else
        {
            if (dt.cam == null)
                GUI.color = warningColor;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cam"));
            GUI.color = defaultColor;
            if (dt.cam == null)
                EditorGUILayout.HelpBox("Camera reference is missing!", MessageType.Error);
        }


        EditorGUILayout.LabelField("Text");
        string text = "—"; // shown when editing multiple objects
        if (!serializedObject.isEditingMultipleObjects)
        {
            if (dt.internal_GetVersion() < 1024 && prefabs) // backward compatibility
                text = dt.internal_GetDeprecatedText();
            else
            {
                if (prefabs)
                    text = dt.serializedText;
                else
                    text = dt.GetText();
            }
        }
        string newText = EditorGUILayout.TextArea(text);
        if (!text.Equals(newText))
        {
            if (serializedObject.isEditingMultipleObjects)
                Undo.RegisterCompleteObjectUndo(targets, "DynamicTexts change");
            else
                Undo.RegisterCompleteObjectUndo(target, "DynamicText change");

            foreach (DynamicText t in targets)
            {
                if (PrefabUtility.GetPrefabType(t) == PrefabType.PrefabInstance)
                {
                    //Debug.Log("Setting new text \"" + newText + "\" for DTPrefabInstance " + t.name);
                    t.SetText(newText);
                    serializedObject.FindProperty("serializedText").stringValue = newText;
                }
                else if (PrefabUtility.GetPrefabType(t) == PrefabType.Prefab)
                {
                    //Debug.Log("Setting new text \"" + newText + "\" for DTPrefab " + t.name);
                    t.SetText(newText);
                    serializedObject.FindProperty("serializedText").stringValue = newText;
                }
                else
                {
                    //Debug.Log("Setting new text \"" + newText + "\" for DTobject " + t.name);
                    t.SetText(newText);
                    serializedObject.FindProperty("serializedText").stringValue = newText; // not sure if this is needed
                }
            }
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("offsetZ"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lineSpacing"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("letterSpacing"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anchor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("alignment"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tabSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fontStyle"));

        if (dt.font == null)
            GUI.color = warningColor;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("font"));
        GUI.color = defaultColor;
        if (dt.font == null)
            EditorGUILayout.HelpBox("Font is missing!", MessageType.Error);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("autoSetFontMaterial"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("color"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baselineRefChar"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("metricsRefChars"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pixelSnapTransformPos"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minFontPxSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxFontPxSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("autoFaceCam"));
        
        serializedObject.ApplyModifiedProperties();
    }
}
