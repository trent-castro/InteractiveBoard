using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.GenericMenu;

[CustomPropertyDrawer(typeof(LayerAttribute))]
class LayerAttributeEditor : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // One line of  oxygen free code.
        property.intValue = EditorGUI.LayerField(position, label, property.intValue);
    }
}
