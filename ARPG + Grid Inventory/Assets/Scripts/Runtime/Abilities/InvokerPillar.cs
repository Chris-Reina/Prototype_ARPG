using System;
using System.Linq;
using UnityEngine;

public class InvokerPillar : Projectile
{
    [HideInInspector] public float damage;
    [SerializeField] private GameObject m_ParticleSystemObject;
    [SerializeField] private LayerMask whatIsCharacter;
    [SerializeField] private float TimerMax;

    public bool IsDamaging { get { return _isDamaging; } }
    public bool IsParticleSystemActive { get { return _isParticleSystemActive; } }

    private bool _isDamaging = false;
    private bool _isParticleSystemActive = false;
    private float _timer = 24f;
    private ParticleSystem m_ParticleSystem;

    private IAttackable _attackable;
    
    private void Awake()
    {
        m_ParticleSystem = m_ParticleSystemObject.GetComponent<ParticleSystem>();

        _timer = TimerMax;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer < 22f && !_isParticleSystemActive)
        {
            m_ParticleSystemObject.gameObject.SetActive(true);
            m_ParticleSystem.Play();
            _isParticleSystemActive = true;
        }         

        if(_timer <= 21f && !_isDamaging)
        {
            _isDamaging = true;
        }

        if(_timer <= 0)
        {
            Destroy(gameObject);
        }

        if(_isDamaging)
            _attackable?.TakeDamage(damage * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayersUtility.PlayerMaskIndex)
        {
            Debug.LogWarning("Choque con algo pLayer");
            _attackable = other.gameObject.GetComponent<IAttackable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayersUtility.PlayerMaskIndex)
        {
            Debug.LogWarning("Choque con algo pLayer Exit");
            _attackable = null;
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public override void Initialize()
    {
        
    }

    public override void Initialize(float initialSpeed)
    {
    }

    public override void Initialize(Vector3 directionalSpeed)
    {
    }

    public override void Initialize(Vector3 direction, float speed)
    {
    }

    public override float GetSpeed()
    {
        return 0f;
    }
}
