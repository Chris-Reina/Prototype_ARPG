using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "AttributeType", menuName = "Item/Attribute/AttributeType")]
public class AttributeType : ScriptableObject
{
    public enum AttributeModificationType
    {
        ModifyMax,
        ModifyMin,
        ModifyBoth
    }
    
    public string attributeName;
    public AttributeModificationType modificationType;
}
