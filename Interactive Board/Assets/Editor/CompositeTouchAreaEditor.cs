using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.GenericMenu;

public enum EInsertType
{
    EMPTY,
    BOX,
    CAPSULE,
    CIRCLE,
    POLYGON,
}

[CustomEditor(typeof(CompositeTouchArea))]
public class CompositeTouchAreaEditor : Editor
{
    SerializedProperty baseAreaProp;
    SerializedProperty touchAreasProp;

    private static int previewLinesPerUnit = 10;
    private static float previewPointTolerance = 0.001f;
    private static bool showTouchAreas = false;
    private static List<bool> showIndividualTouchAreas = new List<bool>();

    private void OnEnable()
    {
        baseAreaProp = serializedObject.FindProperty("BaseArea");
        touchAreasProp = serializedObject.FindProperty("m_touchAreas");

        serializedObject.Update();
        if (touchAreasProp.arraySize == 0)
        {
            touchAreasProp.arraySize = 1;
        }
        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        CompositeTouchArea t = target as CompositeTouchArea;

        if (t == null || t.gameObject == null || baseAreaProp == null)
            return;

        Bounds bounds = t.GetBounds();


        for (float y = bounds.min.y; y <= bounds.max.y; y += 1.0f / previewLinesPerUnit)
        {
            RaycastHit2D[] leftHits = Physics2D.LinecastAll(new Vector2(bounds.min.x, y), new Vector2(bounds.max.x, y));
            RaycastHit2D[] rightHits = Physics2D.LinecastAll(new Vector2(bounds.max.x, y), new Vector2(bounds.min.x, y));

            Handles.color = Color.red;
            foreach (RaycastHit2D hit in leftHits)
            {
                if (t.OverlapPoint(hit.point))
                {
                    Handles.DrawSolidDisc(hit.point, Vector3.forward, 0.01f);
                }
            }

            Handles.color = Color.blue;
            foreach (RaycastHit2D hit in rightHits)
            {
                if (t.OverlapPoint(hit.point))
                {
                    Handles.DrawSolidDisc(hit.point, Vector3.forward, 0.01f);
                }
            }
        }
    }

    private static Editor editor;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Event current = Event.current;

        TouchAreaGUI(current, 0);

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal(GUILayout.Height(18));

        if (touchAreasProp.arraySize != 1)
        {
            showTouchAreas = EditorGUILayout.PropertyField(touchAreasProp, false);
        }
        else
        {
            EditorGUILayout.LabelField("Touch Areas");
        }

        Rect dropDownRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(18));

        if (EditorGUILayout.DropdownButton(new GUIContent("Add"), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            AddInsertTypeMenuItems(menu, touchAreasProp.arraySize - 1, "");
            menu.DropDown(dropDownRect);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();

        if (showTouchAreas && touchAreasProp.arraySize != 1)
        {
            EditorGUI.indentLevel = 1;

            for (int i = 1; i < touchAreasProp.arraySize; i++)
            {
                TouchAreaGUI(current, i);
            }

            EditorGUI.indentLevel = 0;
        }
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Preview Lines Per Unit");
        previewLinesPerUnit = Mathf.Clamp(EditorGUILayout.IntField(previewLinesPerUnit), 0, 100);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Preview Point Tolerance");
        previewPointTolerance = Mathf.Clamp(EditorGUILayout.FloatField(previewPointTolerance), 0.00001f, 0.5f);
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    private void TouchAreaGUI(Event current, int i)
    {

        SerializedProperty touchArea = touchAreasProp.GetArrayElementAtIndex(i);
        string name = i == 0 ? "Base" : touchArea.FindPropertyRelative("area").objectReferenceValue?.name ?? $"Empty Area";

        Rect touchAreaSection = EditorGUILayout.BeginHorizontal();

        if (touchAreaSection.Contains(current.mousePosition) && current.type == EventType.ContextClick)
        {
            CreateTouchAreaContextMenu(current, i, name);
        }

        if (showIndividualTouchAreas.Count <= i)
        {
            showIndividualTouchAreas.Add(false);
        }

        showIndividualTouchAreas[i] = EditorGUILayout.Foldout(showIndividualTouchAreas[i], name, true);

        GUILayout.EndHorizontal();

        if (i < touchAreasProp.arraySize && showIndividualTouchAreas[i])
        {
            EditorGUILayout.PropertyField(touchArea.FindPropertyRelative("area"));

            if (i != 0)
            {
                EditorGUILayout.PropertyField(touchArea.FindPropertyRelative("compositeType"));
            }
        }
    }

    private void CreateTouchAreaContextMenu(Event current, int i, string name)
    {
        GenericMenu menu = new GenericMenu();

        AddInsertTypeMenuItems(menu, i, "Insert/");
        menu.AddSeparator("");

        if (i > 0)
        {
            menu.AddItem(new GUIContent(i == 1 ? $"Swap {name} with Base" : "Move up"), false, CreateMoveTouchAreaCallBack(i, i - 1));
        }
        if (i < touchAreasProp.arraySize - 1)
        {
            menu.AddItem(new GUIContent("Move down"), false, CreateMoveTouchAreaCallBack(i, i + 1));
        }
        if (i != 0)
        {
            menu.AddItem(new GUIContent($"Delete {name}"), false, CreateDeleteTouchAreaCallBack(i));
        }

        menu.ShowAsContext();
    }

    private void AddInsertTypeMenuItems(GenericMenu menu, int i, string path)
    {
        menu.AddItem(new GUIContent($"{path}Empty Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.EMPTY));
        menu.AddSeparator(path);
        menu.AddItem(new GUIContent($"{path}Box Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.BOX));
        menu.AddItem(new GUIContent($"{path}Capsule Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.CAPSULE));
        menu.AddItem(new GUIContent($"{path}Circle Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.CIRCLE));
        menu.AddItem(new GUIContent($"{path}Polygon Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.POLYGON));
    }

    private MenuFunction CreateInsertTouchAreaCallBack(int i, EInsertType type)
    {
        return () =>
        {
            serializedObject.Update();

            InsertTouchArea(i, type);

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void InsertTouchArea(int i, EInsertType type)
    {
        switch (type)
        {
            case EInsertType.EMPTY:
                InsertTouchArea(i, null);
                break;
            case EInsertType.BOX:
                InsertTouchArea<BoxCollider2D>(i, "Box Area");
                break;
            case EInsertType.CAPSULE:
                InsertTouchArea<CapsuleCollider2D>(i, "Capsule Area");
                break;
            case EInsertType.CIRCLE:
                InsertTouchArea<CircleCollider2D>(i, "Circle Area");
                break;
            case EInsertType.POLYGON:
                InsertTouchArea<PolygonCollider2D>(i, "Polygon Area");
                break;
        }
    }

    private void InsertTouchArea<T>(int i, string objectName) where T : Collider2D
    {
        GameObject gameObject = new GameObject(objectName, typeof(T));
        gameObject.transform.parent = (target as CompositeTouchArea).transform;

        InsertTouchArea(i, gameObject);
    }

    private void InsertTouchArea(int i, GameObject gameObject)
    {
        touchAreasProp.InsertArrayElementAtIndex(i + 1);
        showIndividualTouchAreas.Insert(i + 1, false);

        SerializedProperty area = touchAreasProp.GetArrayElementAtIndex(i + 1).FindPropertyRelative("area");
        area.objectReferenceValue = gameObject;
    }

    private MenuFunction CreateDeleteTouchAreaCallBack(int i)
    {
        return () =>
        {
            serializedObject.Update();

            DeleteTouchArea(i);

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void DeleteTouchArea(int i)
    {
        Collider2D area = touchAreasProp.GetArrayElementAtIndex(i).FindPropertyRelative("area").objectReferenceValue as Collider2D;
        if (area != null)
        {
            Destroy(area.gameObject);
        }

        touchAreasProp.DeleteArrayElementAtIndex(i);
        showIndividualTouchAreas.RemoveAt(i);
    }

    private MenuFunction CreateMoveTouchAreaCallBack(int src, int dest)
    {
        return () =>
        {
            serializedObject.Update();

            MoveTouchArea(src, dest);

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void MoveTouchArea(int src, int dest)
    {
        touchAreasProp.MoveArrayElement(src, dest);

        bool temp = showIndividualTouchAreas[src];
        showIndividualTouchAreas[src] = showIndividualTouchAreas[dest];
        showIndividualTouchAreas[dest] = temp;
    }
}
