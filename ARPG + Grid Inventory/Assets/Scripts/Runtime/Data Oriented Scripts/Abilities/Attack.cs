using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Attack", menuName = "Abilities/Attack")]
public class Attack : Ability
{
    public float manaCost = 0;
    public ParticleSystem effectSystem;
    public float damageMultiplicative;

    public float GetDamage([NotNull] Tuple<float, float> damageRange)
    {
        if (damageRange == null) throw new ArgumentNullException(nameof(damageRange));

        var (item1, item2) = new Tuple<float, float>(damageRange.Item1 * damageMultiplicative,
            damageRange.Item2 * damageMultiplicative);
        return Random.Range(item1, item2);
    }
}
