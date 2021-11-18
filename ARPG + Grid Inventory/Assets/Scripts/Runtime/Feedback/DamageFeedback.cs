using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DamageFeedback : MonoBehaviour
{
    [SerializeField] private float _damage = default;
    [SerializeField] private bool _isCrit = default;

    [SerializeField] private Color _normalColor = default;
    [SerializeField] private Color _critColor = default;

    [SerializeField] private TextMeshProUGUI _text = default;
    [SerializeField] private AnimationClip _normalAnimation = default;
    [SerializeField] private AnimationClip _critAnimation = default;
    [SerializeField] private Animation _animation = default;

    public DamageFeedback(bool isCrit, float damage)
    {
        _isCrit = isCrit;
        _damage = damage;
    }

    public void Initialize(float damage, bool isCrit)
    {
        _isCrit = isCrit;
        _damage = damage;
    }
    
    private void Start()
    {
        if (_text == null)
            _text = GetComponent<TextMeshProUGUI>();
        
        if(_animation == null)
            _animation = GetComponent<Animation>();

        _animation.clip = _isCrit ? _critAnimation : _normalAnimation;
        _text.color = _isCrit ? _critColor : _normalColor;
        _text.text = _damage.ToString(CultureInfo.CurrentCulture);

    }

    private void LateUpdate()
    {
        if (!_animation.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}