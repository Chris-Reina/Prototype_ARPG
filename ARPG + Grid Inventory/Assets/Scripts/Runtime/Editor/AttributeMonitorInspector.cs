using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DoaT.Attributes;

[CustomEditor(typeof(AttributeMonitor))]
public class AttributeMonitorInspector : Editor
{
    private AttributeMonitor _monitor;
    private Attribute _attribute;
    private GUIStyle _title = new GUIStyle();

    private void OnEnable()
    {
        _monitor = (AttributeMonitor)target;
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((AttributeMonitor)target), typeof(AttributeMonitor), false);
        GUI.enabled = true;

        GUILayout.Space(10);

        _monitor.attributeManager = (AttributeManager)EditorGUILayout.ObjectField("Attribute Manager", _monitor.attributeManager, typeof(AttributeManager), true);

        _monitor.attributeName = EditorGUILayout.TextField(new GUIContent("Attribute Name", "Name of the attribute that is going to be searched by the Attribute Manager"), _monitor.attributeName);

        _monitor.barFill = (Image)EditorGUILayout.ObjectField("Bar Image", _monitor.barFill, typeof(Image), true);

        EditorGUILayout.Space();

        var temp = _monitor.updateType;
        _monitor.updateType = (AttributeMonitor.BarUpdateType)EditorGUILayout.EnumPopup("Bar Update Type", _monitor.updateType);

        if (temp == AttributeMonitor.BarUpdateType.FillAmount && temp != _monitor.updateType)
            _monitor.CheckSlider();

        if (_monitor.updateType == AttributeMonitor.BarUpdateType.Slider)
        {
            _monitor.slider = (Slider)EditorGUILayout.ObjectField("Slider", _monitor.slider, typeof(Slider), true);
        }

        if(_monitor.barFill != null)
        {
            EditorGUILayout.Space();

            //Debug.Log(EditorGUILayout.ColorField(new GUIContent("Bar Color", ""), _monitor.barFill.color, true, true, false));

            _monitor.barColor = EditorGUILayout.ColorField(new GUIContent("Bar Color", ""), _monitor.barColor, true, true, false);
            _monitor.ChangeBarColor();
        }
    }
}
