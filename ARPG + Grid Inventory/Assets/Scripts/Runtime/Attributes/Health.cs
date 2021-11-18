using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoaT.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private string healthID = "Health";
        [SerializeField] private bool Invulnerable = false;
        [SerializeField] private bool Undying = false;
        
        private AttributeManager _manager;
        private Attribute _healthAttribute;
        [SerializeField] private bool _isDead;

        public bool IsDead
        {
            get
            {
                _isDead = _healthAttribute.ValueIsMinimum;
                return _isDead;
            }
        }
        public float HealthAmount => _healthAttribute.ValueRatio;

        public event Action OnDeath;
        public event Action OnHealthRefill;

        private void Awake()
        {
            _manager = GetComponent<AttributeManager>();
        }

        private void Start()
        {
            _healthAttribute = _manager.TryGetAttribute(healthID);
        }

        public void TakeDamage(float amount)
        {
            if (Invulnerable) return;
            _healthAttribute.AddValue(-amount);
            if(Undying)
                if (_healthAttribute.ValueIsMinimum)
                    _healthAttribute.Value = 1f;
            
            if(IsDead) OnDeath?.Invoke();
        }

        public void Heal(float amount)
        {
            _healthAttribute.AddValue(amount);
        }

        public void RefillHealth()
        {
            _healthAttribute.ResetValueToMax();
            OnHealthRefill?.Invoke();
        }
    }
    
}

