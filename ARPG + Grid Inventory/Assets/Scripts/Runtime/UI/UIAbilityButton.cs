using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIAbilityButton : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI iconKeyText;

    public int abilityIndex;
    public InputKeys keys;
    public PlayerAbilityData data;

    private void LateUpdate()
    {
        if (abilityIndex > data.abilities.Count) return;

        if (abilityIndex == 0)
        {
            iconImage.color = Color.white;
            iconImage.sprite = data.cursorMain.icon;
            iconKeyText.text = keys.Abilities[abilityIndex].ToString();
        }
        else
        {
            iconKeyText.text = keys.Abilities[abilityIndex].ToString();
            
            if (data.abilities[abilityIndex-1] == null)
            {
                iconImage.color = Color.clear;
                return;
            }

            iconImage.color = Color.white;
            iconImage.sprite = data.abilities[abilityIndex-1].icon;
        }
    }
}
