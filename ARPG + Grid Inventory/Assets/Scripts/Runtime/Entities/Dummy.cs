using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using DoaT.Attributes;
using UnityEngine;

public class Dummy : MonoBehaviour, IEntity, IAttackable
{
    public string Name => name;
    public float Durability => _health.HealthAmount;
    public Vector3 Position => transform.position;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public bool IsEnemy => true;
    public bool IsDead => _health.IsDead;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
        EventManager.Trigger(EventsData.OnEntityDamageTaken, transform.position, this, amount, false);
    }
}
