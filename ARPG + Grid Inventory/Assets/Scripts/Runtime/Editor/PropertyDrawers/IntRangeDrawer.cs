using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IntRange))]
public class IntRangeDrawer : PropertyDrawer
{
    private float FieldWidthPercentage => 1f - SliderWidthPercentage;
    private const float PaddingPercentage = 0.001f;
    private const int NumberOfFields = 4;
    private const float SliderWidthPercentage = 0.46f;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var min = property.FindPropertyRelative("min");
        var max = property.FindPropertyRelative("max");
        var minRelative = property.FindPropertyRelative("relativeMin");
        var maxRelative = property.FindPropertyRelative("relativeMax");

        float tempMin = (float)min.intValue;
        float tempMax = (float)max.intValue;

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        var padding = position.width * PaddingPercentage;
        var valuesWidth = position.width * ((FieldWidthPercentage / NumberOfFields) - PaddingPercentage);
        var sliderWidth = position.width * SliderWidthPercentage;

        var positionCumulative = position.x;
        var minRelRect = new Rect(positionCumulative, position.y, valuesWidth, position.height);
        positionCumulative += (valuesWidth + padding);
        var minRect = new Rect(positionCumulative, position.y, valuesWidth , position.height);
        positionCumulative += (valuesWidth + padding*2);
        var sliderRect = new Rect(positionCumulative + padding, position.y, sliderWidth, position.height);
        positionCumulative += (sliderWidth + padding*2);
        var maxRect = new Rect(positionCumulative, position.y, valuesWidth, position.height);
        positionCumulative += (valuesWidth + padding);
        var maxRelRect = new Rect(positionCumulative, position.y, valuesWidth, position.height);

        EditorGUI.PropertyField(minRect, min, GUIContent.none);
        EditorGUI.PropertyField(maxRect, max, GUIContent.none);
        EditorGUI.PropertyField(minRelRect, minRelative, GUIContent.none);
        EditorGUI.PropertyField(maxRelRect, maxRelative, GUIContent.none);

        var minBefore = tempMin;
        var maxBefore = tempMax;
        //EditorGUI.MinMaxSlider(sliderRect,ref tempMin, ref tempMax, minRelative.floatValue, maxRelative.floatValue);
        EditorGUI.MinMaxSlider(sliderRect,ref tempMin, ref tempMax, minRelative.intValue, maxRelative.intValue);
        
        min.intValue = (int)tempMin != (int)minBefore ? (int)tempMin : min.intValue;
        max.intValue = (int)tempMax != (int)maxBefore ? (int)tempMax : max.intValue;

        EditorGUI.EndProperty();
    }
}