
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CursorRaycastResult
{
    public Vector3 Point { get; protected set; }
}

[Serializable]
public class InteractableRaycastResult : CursorRaycastResult
{
    public IInteractable InteractableObject { get; private set; }

    public InteractableRaycastResult(Vector3 hitPoint, IInteractable interactable)
    {
        Point = hitPoint;
        InteractableObject = interactable;
    }
}

[Serializable]
public class EntityRaycastResult : CursorRaycastResult
{
    public bool IsEnemy { get; private set; }
    public bool IsDead { get; private set; }
    public IEntity Entity { get; private set; }
    
    public EntityRaycastResult(Vector3 hitPoint, IEntity entity)
    {
        Point = hitPoint;
        Entity = entity;
        IsEnemy = entity.IsEnemy;
        IsDead = entity.IsDead;
    }
}

[Serializable]
public class MovementRaycastResult : CursorRaycastResult
{
    public MovementRaycastResult(Vector3 hitPoint)
    {
        Point = hitPoint;
    }
}

[Serializable]
public class UIRaycastResult : CursorRaycastResult
{
    public ITargetableUI Target { get; private set; }

    public UIRaycastResult(ITargetableUI target, Vector3 hitPoint)
    {
        Point = hitPoint;
        Target = target;
    }
}
