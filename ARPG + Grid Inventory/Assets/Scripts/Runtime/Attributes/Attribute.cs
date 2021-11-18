using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace DoaT.Attributes
{
    [System.Serializable]
    public class Attribute
    {
        [Tooltip("The name of the attribute.")]
        [SerializeField] private string m_Name;
        [Tooltip("The minimum value of the attribute.")]
        [SerializeField] private float m_MinValue;
        [Tooltip("The maximum value of the attribute.")]
        [SerializeField] private float m_MaxValue = 100;
        [Tooltip("The current value of the attribute.")]
        [SerializeField] private float m_Value = 100;
    
        public string Name => m_Name;
        public float MinValue 
        { 
            get => m_MinValue;
            set => m_MinValue = value;
        }
        public float MaxValue 
        { 
            get => m_MaxValue;
            set => m_MaxValue = value;
        }    
        public float Value
        {
            get => m_Value;
            set => m_Value = Mathf.Clamp(value, m_MinValue, m_MaxValue);
        }

        public bool ValueIsMinimum => m_Value == m_MinValue;
        public bool ValueIsMaximum => m_Value == m_MaxValue;
        public float ValueRatio => Value/MaxValue;

        /// <summary>
        /// Base Constructor.
        /// </summary>
        public Attribute() { }
        
        /// <summary>
        /// Primary Constructor.
        /// </summary>
        /// <param name="name">Name of the Attribute.</param>
        /// <param name="value">Max and Initial Value of the Attribute. (Optional)</param>
        public Attribute(string name, float value = 100)
        {
            m_Name = name;
            m_Value = m_MaxValue = value;
        }
    
        /// <summary>
        /// Resets the Value to the Maximum Value.
        /// </summary>
        public void ResetValueToMax()
        {
            m_Value = m_MaxValue;
        }
    
        public void AddValue(float addedValue)
        {
            Value += addedValue;
        }
    
        public void Rename(string newName)
        {
            m_Name = newName;
        }

        public bool CanSpend(float amount)
        {
            return (m_Value - amount) >= 0;
        }
    }
}

