using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

using DoaT.Attributes;
using Attribute = DoaT.Attributes;

[CustomEditor(typeof(AttributeManager))]
public class AttributeManagerInspector : Editor
{
    AttributeManager m_AttributeManager;
    ReorderableList m_AttributeReorderableList;

    int m_attributeSelectedIndex = -1;
    bool m_shouldDrawAttributeValues = false;
    bool m_shouldShowExtendedDataOnList = false;

    private void OnEnable()
    {
        m_AttributeManager = (AttributeManager)target;

        m_AttributeReorderableList = new ReorderableList(serializedObject,serializedObject.FindProperty("m_Attributes"),false,true,true,true);

        m_AttributeReorderableList.drawElementCallback = DrawElementCallback;
        m_AttributeReorderableList.drawHeaderCallback = DrawHeaderCallback;
        m_AttributeReorderableList.onRemoveCallback = OnRemoveCallback;
        m_AttributeReorderableList.onSelectCallback += OnSelectCallback;
        m_AttributeReorderableList.onAddCallback += OnAddCallback;
    }
    
    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, "Attributes");
    }
    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
    {
        var temp = m_AttributeManager.m_Attributes[index];

        if (m_shouldShowExtendedDataOnList)
        {
            var label = temp.Name + " (Value: " + temp.Value + ") (Ratio: " + (temp.ValueRatio * 100).ToString("0.00") + "%)";
            EditorGUI.LabelField(rect, label);
        }
        else
        {
            var label = m_AttributeManager.m_Attributes[index].Name;
            EditorGUI.LabelField(rect, label);
        }
       

        if (index == m_attributeSelectedIndex)
        {
            m_shouldDrawAttributeValues = isactive;
        }

    }
    private void OnAddCallback(ReorderableList list)
    {
        m_AttributeManager.AddAttribute("New Attribute");       
    }
    private void OnRemoveCallback(ReorderableList list)
    {
        m_AttributeManager.RemoveAttribute(list.index);
    }
    private void OnSelectCallback(ReorderableList list)
    {
        m_attributeSelectedIndex = list.index;

        //Debug.Log("seleccioné en el inspector indice: " + list.index);
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((AttributeManager)target), typeof(AttributeManager), false);
        GUI.enabled = true;

        GUILayout.Space(10);

        m_shouldShowExtendedDataOnList = EditorGUILayout.Toggle(new GUIContent("Show Extended Values", "Extended Values are shown in the list below."), m_shouldShowExtendedDataOnList);

        GUILayout.Space(5);

        serializedObject.Update();

        m_AttributeReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();

        DrawAttributeValues(m_attributeSelectedIndex);
    }        

    private void DrawAttributeValues(int index)
    {
        if (!m_shouldDrawAttributeValues || m_AttributeManager.m_Attributes.Count == 0)        
            return;

        if (m_attributeSelectedIndex >= m_AttributeManager.m_Attributes.Count)
            m_attributeSelectedIndex = m_AttributeManager.m_Attributes.Count - 1;

        EditorGUI.indentLevel++; //Indentation ++
        GUILayout.Space(5);

        var temp = m_AttributeManager.m_Attributes[m_attributeSelectedIndex];

        var newName = EditorGUILayout.TextField(new GUIContent("Name", "The name of the attribute."), temp.Name);
        if (temp.Name != newName)
        {
            m_AttributeManager.RenameAttribute(temp.Name, newName);
        }

        float minValue = EditorGUILayout.FloatField(new GUIContent("MinValue", "Minimum value achievable by the attribute."), temp.MinValue);        
        if (minValue > temp.MaxValue)
        {
            temp.MaxValue = minValue;
        }
        temp.MinValue = minValue;
        
        
        float maxValue = EditorGUILayout.FloatField(new GUIContent("MaxValue", "Maximum value achievable by the attribute."), temp.MaxValue);
        if (maxValue < temp.MinValue)
        {
            temp.MinValue = maxValue;
        }
        temp.MaxValue = maxValue;

        var value = EditorGUILayout.FloatField(new GUIContent("Value", "Current value of the attribute."), temp.Value);
        if (temp.MaxValue < value)
        {
            value = temp.MaxValue;
        }
        else if (temp.MinValue > value)
        {
            value = temp.MinValue;
        }
        if (value > temp.MaxValue)
        {
            temp.MaxValue = value;
        }
        else if (value < temp.MinValue)
        {
            temp.MinValue = value;
        }
        temp.Value = value;

        EditorGUI.indentLevel--; //Indentation --
    }


    
}
