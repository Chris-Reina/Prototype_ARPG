using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable,CreateAssetMenu(fileName = "ItemWeapon",menuName = "Item/Weapon")]
public class ItemWeapon : Item
{
    public WeaponType type;
    public Vector2 damage;
    public float criticalChance;
}
