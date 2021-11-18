using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthMonitor : MonoBehaviour
{
    [SerializeField] private GameSelectionManager _selectionManager = default;
    [SerializeField] private CanvasGroup _healthGroup = default;
    [SerializeField] private Image _healthImage = default;
    [SerializeField] private TextMeshProUGUI _healthText = default;

    private void Awake()
    {
        if (_selectionManager == null)
            _selectionManager = FindObjectOfType<GameSelectionManager>();

        if (_healthGroup == null)
            _healthGroup = GetComponent<CanvasGroup>();
    }

    private void LateUpdate() //WIP
    {
        if (_selectionManager.CursorSelection is EntityRaycastResult selection)
        {
            if (selection.IsDead)
            {
                _healthGroup.alpha = 0;
            }
            else
            {
                _healthGroup.alpha = 1;
                _healthImage.fillAmount = selection.Entity.Durability;
                _healthText.text = selection.Entity.Name;
            }
        }
        else
        {
            _healthGroup.alpha = 0;
        }
    }
}
