using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable,CreateAssetMenu(fileName = "PlayerAbilityData",menuName = "Data/Player/PlayerAbilityData")]
public class PlayerAbilityData : ScriptableObject
{
    public Ability cursorMain;
    public Ability cursorDefault;
    public MovementAbility movementDefault;

    public List<Ability> abilities;

    public Ability GetAbility(int index)
    {
        if (index < 0 || index > abilities.Count) return null;

        if (index == 0)
        {
            return cursorMain ? cursorMain : cursorDefault;
        }

        return abilities[index-1];
    }
}
