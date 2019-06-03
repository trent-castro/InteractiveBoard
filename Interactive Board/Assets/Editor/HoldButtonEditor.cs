using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using static UnityEditor.GenericMenu;

[CustomEditor(typeof(HoldButton))]
public class HoldButtonEditor : ButtonEditor
{
    SerializedProperty OnDownProp;
    SerializedProperty OnUpProp;

    protected override void OnEnable()
    {
        base.OnEnable();

        OnDownProp = serializedObject.FindProperty("onDown");
        OnUpProp = serializedObject.FindProperty("onUp");
    }


    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(OnDownProp);
        EditorGUILayout.PropertyField(OnUpProp);

        serializedObject.ApplyModifiedProperties();
    }
}