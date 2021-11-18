using System;
using System.Collections.Generic;
using System.Linq;
using DoaT.AI;
using UnityEngine;

public class AbilityEffectData : MonoBehaviour
{
    //public delegate void AbilityAction(Ability)
    
    private static Dictionary<uint, Action<Ability, Model>> _abilityById;

    public static Action<IEntity> MarkAbility;
    public static Action<IEntity> MarkSwitch;
    public static Action<IEnergySensitive> MarkExplode;

    public static Dictionary<uint, Action<Ability, Model>> AbilityById => _abilityById;
    
    private void Awake()
    {
        _abilityById = new Dictionary<uint, Action<Ability, Model>>
        {
            {0, PlayerBasicAttack},
            {1, PlayerBasicMovement},
            {2, PlayerSlash},
            {3, ZombieBasicAttack},
            {4, ArcherArrowSpawn},
            {5, MiniBossBasicAttack},
            {6, MiniBossAreaSpawn}
        };

        MarkAbility += MarkAbilityEffect;
        MarkSwitch += MarkRotationEffect;
        MarkExplode += MarkExplodeEffect;

    }

    private void PlayerBasicAttack(Ability ability, Model model)
    {
        var concreteModel = (PlayerModel) model;

        if (Physics.SphereCast(new Ray(concreteModel.RayInitPosition, concreteModel.RotationDirectionNormalized), 0.5f,
            out var hit, ability.range + 0.1f, LayersUtility.EntityMask))
        {
            var attackable = hit.collider.GetComponent<IAttackable>();
            var attack = ability as Attack;
            if (attack != null) attackable.TakeDamage(attack.GetDamage(concreteModel.Inventory.WeaponDamage));
        }
    }
    private void PlayerBasicMovement(Ability ability, Model model)
    {

        var concreteModel = (PlayerModel) model;
        var pc = concreteModel.controller;

        if (pc.path.Count <= 0) return;
        if (pc.currentIndex >= pc.path.Count) return;
        
        concreteModel.RotationPoint = pc.path[pc.currentIndex];

        if(Vector3.Distance(pc.path[pc.currentIndex], pc.Position) < concreteModel.nodeDetection)// || InCorrectDirection())
            pc.currentIndex++;


        if (pc.currentIndex < pc.path.Count)
        {
            pc.transform.position += (pc.path[pc.currentIndex] - pc.transform.position).normalized *
                                     (pc.Model.movementSpeed * Time.deltaTime);

            pc.UpdatePosition();
        }


        // bool InCorrectDirection() TODO: Implement
        // {
        //     var current = (pc.path[pc.currentIndex] - pc.Position).normalized;
        //     var next = pc.path.NextNodeDirection(pc.currentIndex);
        //
        //     return false;
        // }
    }
    private void ZombieBasicAttack(Ability ability, Model model)
    {
        var concreteModel = (ZombieModel) model;
        
        if (Physics.SphereCast(new Ray(concreteModel.RayInitPosition, concreteModel.RotationDirectionNormalized), 0.3f,
            out var hit, ability.range + 0.1f, LayersUtility.PlayerMask, QueryTriggerInteraction.Collide))
        {
            var attackable = hit.collider.GetComponent<IAttackable>();
            var attack = ability as Attack;
            if (attack != null) attackable.TakeDamage(attack.GetDamage(concreteModel.data.GetDamageRange()));
        }
    }
    private void PlayerSlash(Ability ability, Model model)
    {
        var concreteModel = (PlayerModel) model;
        var attack = ability as Attack;

        var result = Physics.OverlapSphere(concreteModel.Position, ability.range, LayersUtility.EntityMask,
            QueryTriggerInteraction.Collide).Select(x => x.gameObject).ToList();
        
        if (result.Count > 0)
        {
            foreach (var attackable in result
                .Where(gO => Vector3.Angle(gO.transform.position - concreteModel.Position, concreteModel.transform.forward) <= 75)
                .Select(gO => gO.GetComponent<IAttackable>())
                .Where(attackable => attackable != null))
            {
                attackable.TakeDamage(attack.GetDamage(concreteModel.Inventory.WeaponDamage));
            }
        }
    }
    private void MarkAbilityEffect(IEntity entity)
    {
        var eR = entity.GameObject.GetComponent<IEnergyReceiver>();

        if (eR != null)
        {
            EnergyOrbManager.Instance.SetTarget(EnergyOrbManager.Instance.target == eR ? null : eR);
        }
    }

    private void MarkRotationEffect(IEntity entity)
    {
        EnergyHolder.Instance.NextEnergySocket();
    }
    
    private void MarkExplodeEffect(IEnergySensitive sensitive)
    {
        var sense = EnergyOrbManager.Instance.target;
        
        var done = false;
        if (sense is IEnergySensitive eS)
        {
            done = eS.Explode(EnergyHolder.Instance.CurrentIndexEnergy);
        }
        EnergyHolder.Instance.ExplodeResponse(done);
        EnergyOrbManager.Instance.ExplodeResponse(done);
    }

    private void ArcherArrowSpawn(Ability ability, Model model)
    {
        var concreteModel = (ArcherModel) model;
        var attack = (RangedAttack) ability;

        var proj = (Arrow)Instantiate(attack.prefab, concreteModel.spawnPoint.position, concreteModel.transform.rotation);
        proj.SetDamage(attack.GetDamage(concreteModel.data.GetDamageRange()))
            .Initialize(concreteModel.transform.forward, attack.prefab.GetSpeed());
    }
    
    private void MiniBossBasicAttack(Ability ability, Model model)
    {
        var concreteModel = (MiniBossModel) model;
        var attack = ability as Attack;
        
        var result = Physics.OverlapSphere(concreteModel.Position, ability.range, LayersUtility.PlayerMask,
            QueryTriggerInteraction.Collide).Select(x => x.gameObject).ToList();
        
        if (result.Count > 0)
        {
            foreach (var attackable in result
                .Where(gO => Vector3.Angle(gO.transform.position - concreteModel.Position, concreteModel.transform.forward) <= 120)
                .Select(gO => gO.GetComponent<IAttackable>())
                .Where(attackable => attackable != null))
            {
                attackable.TakeDamage(attack.GetDamage(concreteModel.data.GetDamageRange()));
            }
        }

        // if (Physics.SphereCast(new Ray(concreteModel.RayInitPosition, concreteModel.RotationDirectionNormalized), 0.6f,
        //     out var hit, ability.range + 0.1f, LayersUtility.PlayerMask, QueryTriggerInteraction.Collide))
        // {
        //     var attackable = hit.collider.GetComponent<IAttackable>();
        //     var attack = ability as Attack;
        //     if (attack != null) attackable.TakeDamage(attack.GetDamage(concreteModel.data.GetDamageRange()));
        // }
    }
    
    private void MiniBossAreaSpawn(Ability ability, Model model)
    {
        var concreteModel = (MiniBossModel) model;
        var attack = (RangedAttack) ability;

        var proj = (InvokerPillar)Instantiate(attack.prefab, concreteModel.targetData.Position, Quaternion.identity);
        proj.SetDamage(attack.GetDamage(concreteModel.data.GetDamageRange()));
    }
}
