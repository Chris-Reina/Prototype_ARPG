using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoaT.Attributes;

public class AttributeMonitor : MonoBehaviour
{
    public enum BarUpdateType
    {
        FillAmount,
        Slider,
    }

    public AttributeManager attributeManager;
    public Image barFill;
    public Slider slider;
    public string attributeName = "Health";
    public Color barColor = Color.green;
    public BarUpdateType updateType;
    private Attribute _attribute;


    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (barFill == null)
        {
            Debug.LogError("There should be a Bar Fill Asigned");
        }

        if (slider != null)
        {
            slider.fillRect = barFill.rectTransform;
        }
    }

    private void Start()
    {
        if(attributeManager != null)
        {
            GetAttribute();
        }

        CheckSlider();
    }

    private void Update()
    {

        if (_attribute == null)
        {
            GetAttribute();
            return;
        }


        if (updateType == BarUpdateType.FillAmount)
        {
            if (barFill == null) return;

            barFill.fillAmount = _attribute.ValueRatio;
        }
        else if (updateType == BarUpdateType.Slider)
        {
            if (slider == null || barFill == null) return;
            if (slider.fillRect == null)
            {
                slider.fillRect = barFill.rectTransform;
            }

            slider.value = _attribute.ValueRatio;
        }
    }

    public void GetAttribute()
    {
        var temp = attributeManager.TryGetAttribute(attributeName);

        if (temp != null)
            _attribute = temp;
    }

    public void ChangeBarColor()
    {
        barFill.color = barColor;
    }

    public void CheckSlider()
    {
        if (updateType == BarUpdateType.Slider && slider == null) 
        {
            gameObject.AddComponent<Slider>();
            slider = GetComponent<Slider>();
            slider.transition = Selectable.Transition.None;
            slider.fillRect = barFill.rectTransform;
        }
    }

#region Builder
    public AttributeMonitor SetAttributeManager(AttributeManager manager)
    {
        attributeManager = manager;
        return this;
    }

    public AttributeMonitor SetAttributeName(string name)
    {
        attributeName = name;
        return this;
    }

    public AttributeMonitor SetBarFillImage(Image image)
    {
        barFill = image;
        return this;
    }

    public AttributeMonitor SetBarColor(Color color)
    {
        barColor = color;
        return this;
    }

    public AttributeMonitor SetBarUpdateType(BarUpdateType barType)
    {
        updateType = barType;
        return this;
    }

#endregion
}
