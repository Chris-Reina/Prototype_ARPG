using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DoaT;
using UnityEngine;

public class InputCommandManager : MonoBehaviour, IUpdate
{
    public event Action<CharacterCommand> OnCommandCall;

    public PlayerStreamedData playerData;
    public CursorGameSelection selection;
    public PlayerAbilityData abilityData;
    public PlayerModel model;
    
    [Range(0f, 1f), SerializeField] private float raycastCDBase = 0.12f;

    private float _raycastCD;

    private void Awake()
    {
        if (model == null)
            model = FindObjectOfType<PlayerModel>();
        
        _raycastCD = raycastCDBase;
    }

    private void Start()
    {
        UpdateManager.AddUpdate(this);
    }

    public void OnUpdate()
    {
        var forced = InputManager.ForcePositionKey(GetKeyType.GetKey);
        var abilityIndexPressed = InputManager.GetAbilityKey(out var mouseIndexPressed,GetKeyType.GetKeyDown);
        var abilityIndexHold = InputManager.GetAbilityKey(out var mouseIndexHold, GetKeyType.GetKey);
        

        if (CursorPressed(mouseIndexPressed, forced))
        {
            //Debug.Log("CursorPressed");
            return;
        }
        if (AbilityPressed(abilityIndexPressed, forced))
        {
            //Debug.Log("AbilityPressed");
            return;
        }
        
        if (_raycastCD > 0)
        {
            _raycastCD -= Time.deltaTime;
            return;
        }
        
        if(CursorHold(mouseIndexHold, forced))
        {
            //Debug.Log("CursorHold");
            return;
        }
        if(AbilityHold(abilityIndexHold, forced))
        {
            //Debug.Log("AbilityHold");
            return;
        }
    }
    

    private bool CursorPressed(int mouseIndex, bool forced)
    {
        if (!InputManager.CursorMain(GetKeyType.GetKeyDown) || forced) return false;

        _raycastCD = raycastCDBase;
        
        if (selection.IsItemInCursor && !(selection.raycastResult is UIRaycastResult))
        {
            EventManager.Trigger(EventsData.OnWorldLootSpawn, playerData.Position, selection.ItemInCursor);
            selection.ItemInCursor = null;
            return true;
        }

        switch (selection.raycastResult)
        {
            case UIRaycastResult uiResult:
                ClickOverUi(uiResult);
                break;
            case EntityRaycastResult entityResult:
                ClickOverEntity(entityResult, mouseIndex, true);
                break;
            case InteractableRaycastResult interactResult:
                ClickOverInteractable(interactResult);
                break;
            case MovementRaycastResult movementResult:
                ClickOverTraversable(movementResult, true);
                EventManager.Trigger(EventsData.OnWorldClick, movementResult.Point);
                break;
        }

        return true;
    }
    private bool AbilityPressed(int index, bool forced)
    {
        if (index == -1 || model.IsInAbility) return false;

        _raycastCD = raycastCDBase;
        var ability = abilityData.GetAbility(index);

        if (ability == default) return false;
        
        if (ability is MovementAbility mAbility)
        {
            OnCommandCall?.Invoke(
                new AbilityCommand(selection.raycastResult.Point, forced)
                    .SetAbility(null)
                    .SetMovementAbility(mAbility, true, true));
        }
        else
        {
            OnCommandCall?.Invoke(
                new AbilityCommand(selection.raycastResult.Point, forced)
                    .SetAbility(ability)
                    .SetDefaultAttack(abilityData.cursorDefault as Attack)
                    .SetMovementAbility(abilityData.movementDefault, false, true));
        }

        return true;
    }
    private bool CursorHold(int mouseIndex, bool forced)
    {
        if (!InputManager.CursorMain(GetKeyType.GetKey) || forced || model.IsInAbility) return false;
        
        _raycastCD = raycastCDBase;

        switch (selection.raycastResult)
        {
            case UIRaycastResult uiResult:
                ClickOverUi(uiResult);
                break;
            case EntityRaycastResult entityResult:
                ClickOverEntity(entityResult, mouseIndex, true);
                break;
            case InteractableRaycastResult interactResult:
                ClickOverInteractable(interactResult);
                break;
            case MovementRaycastResult movementResult:
                ClickOverTraversable(movementResult, true);
                break;
        }

        return true;
    }
    private bool AbilityHold(int index, bool forced)
    {
        if (index == -1 || model.IsInAbility) return false;

        _raycastCD = raycastCDBase;
        var ability = abilityData.GetAbility(index);

        if (ability == default) return false;
        
        if (ability is MovementAbility mAbility)
        {
            OnCommandCall?.Invoke(
                new AbilityCommand(selection.raycastResult.Point, forced)
                    .SetAbility(null)
                    .SetMovementAbility(mAbility, true, true));
        }
        else
        {
            OnCommandCall?.Invoke(
                new AbilityCommand(selection.raycastResult.Point, forced)
                    .SetAbility(ability)
                    .SetDefaultAttack(abilityData.cursorDefault as Attack)
                    .SetMovementAbility(abilityData.movementDefault, false, true));
        }

        return true;
    }


    private void ClickOverUi(UIRaycastResult result)
    {
        result.Target?.OnClick();
    }
    private void ClickOverEntity(EntityRaycastResult result, int mouseIndex, bool fullMovement)
    {
        var ability = abilityData.GetAbility(mouseIndex);

        OnCommandCall?.Invoke(new AbilityCommand(result.Point, false)
            .SetAbility(ability)
            .SetDefaultAttack(abilityData.cursorDefault as Attack)
            .SetMovementAbility(abilityData.movementDefault, ability is MovementAbility, fullMovement));
    }
    private void ClickOverInteractable(InteractableRaycastResult result) //WIP
    {
        OnCommandCall?.Invoke(new InteractCommand(result.Point, false)
                            .SetInteractable(result.InteractableObject)
                            .SetMovementAbility(abilityData.movementDefault, true));
    }
    private void ClickOverTraversable(MovementRaycastResult result, bool fullMovement)
    {
        if (Vector3.Distance(result.Point, model.transform.position) < model.minMoveDistance)
        {
            //Debug.Log($"Changed Rotation Point. InputCommandManager->ClickOverTraversable");
            model.RotationPoint = result.Point;
            return;
        }

        OnCommandCall?.Invoke(
            new AbilityCommand(result.Point, false).SetMovementAbility(abilityData.movementDefault, true, fullMovement));
    }
    
    
}
