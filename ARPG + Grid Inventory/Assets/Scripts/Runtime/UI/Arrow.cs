using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _speed = 10;

    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        if (!_rb) _rb = GetComponent<Rigidbody>();
    }

    #region Initialize
    public override void Initialize()
    {
        Initialize(transform.forward, _speed);
    }
    public override void Initialize(float initialSpeed)
    {
        Initialize(transform.forward, initialSpeed);
    }
    public override void Initialize(Vector3 directionalSpeed)
    {
        Initialize(directionalSpeed, 1);
    }
    public override void Initialize(Vector3 direction, float speed)
    {
        _rb.velocity = direction * speed;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case LayersUtility.WallMaskIndex:
                Destroy(gameObject);
                break;
            case LayersUtility.PlayerMaskIndex:
                other.gameObject.GetComponent<IAttackable>().TakeDamage(_damage);
                Destroy(gameObject);
                break;
            default:
                return;
        }
    }

    public Arrow SetDamage(float damage)
    {
        _damage = damage;
        return this;
    }

    public override float GetSpeed()
    {
        return _speed;
    }
}
