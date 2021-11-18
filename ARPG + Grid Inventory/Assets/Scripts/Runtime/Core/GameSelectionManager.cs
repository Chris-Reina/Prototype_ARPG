using System;
using System.Collections.Generic;
using System.Linq;
using DoaT;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlaneManager))]
public class GameSelectionManager : MonoBehaviour, IUpdate
{
    [SerializeField] private Camera _camera;
    [SerializeField] private PlaneManager _plane;
    [SerializeField] private ITargetableUI currentPointerTarget;

    public CursorGameSelection selection;
    public InventoryData inventoryData;
    
    public ITargetableUI CurrentPointerTarget => currentPointerTarget;
    
    public CursorRaycastResult CursorSelection
    {
        get => selection.raycastResult;
        private set => selection.raycastResult = value;
    }

    private void Awake()
    {
        if(_camera == null)
            _camera = Camera.main;

        if (_plane == null)
            _plane = GetComponent<PlaneManager>();

        CursorSelection = new MovementRaycastResult(Vector3.zero);
    }
    
    private void Start()
    {
        UpdateManager.AddUpdate(this);
        
        EventManager.Subscribe(EventsData.OnSlotPointerEnter, AssignCurrentPointerTarget);
        EventManager.Subscribe(EventsData.OnSlotPointerExit, ReleaseCurrentPointerTarget);
        
        inventoryData.PointerOnUI += HasPointerTarget;
        selection.GetPointerTarget += GetPointerTarget;
    }

    public void OnUpdate()
    {
        if (InputManager.CursorMain(GetKeyType.GetKeyDown) && !EventSystem.current.IsPointerOverGameObject())
            selection.extendedMovement = true;
        else if (InputManager.CursorMain(GetKeyType.GetKeyUp))
            selection.extendedMovement = false;
        
        
        CursorSelection = GetRaycastResult(_camera.ScreenPointToRay(Input.mousePosition));
    }


    private CursorRaycastResult GetRaycastResult(Ray ray)
    {
        if (EventSystem.current.IsPointerOverGameObject() && !selection.extendedMovement) // UI
        {
            Vector3 result;
            
            if(Physics.Raycast(ray, out var world, 100f, LayersUtility.TraversableMask))
            {
                result = world.point;
            }
            else
            {
                _plane.Plane.Raycast(ray, out var distance);
                result = ray.GetPoint(distance);
            }
            
            return new UIRaycastResult(currentPointerTarget, result);
        }

        if (!Physics.Raycast(ray, out var hit, 100f, LayersUtility.CursorSelectorMask)) //if the raycast hit nothing it will return a Plane based Movement Result.
        {
            _plane.Plane.Raycast(ray, out var result);
            return new MovementRaycastResult(ray.GetPoint(result));
        }

        var layer = hit.transform.gameObject.layer;

        if (layer == LayersUtility.InteractableMaskIndex || layer == LayersUtility.WorldItemMaskIndex)  //INTERACTABLE
        {
           var interactable = hit.transform.parent.GetComponentInChildren<IInteractable>();
           
            return new InteractableRaycastResult(hit.point, interactable);
        }

        if (layer == LayersUtility.EntityMaskIndex)
        {
            var entity = hit.transform.GetComponent<IEntity>();
            return new EntityRaycastResult(entity.Position, entity);
            //return new EntityRaycastResult(hit.point, entity);
        }

        return layer == LayersUtility.TraversableMaskIndex ? new MovementRaycastResult(hit.point) : null;
    }





    private ITargetableUI GetPointerTarget()
    {
        return currentPointerTarget;
    }
    
    private bool HasPointerTarget()
    {
        return currentPointerTarget != null;
    }

    #region Events
    private void AssignCurrentPointerTarget(params object[] parameters)
    {
        currentPointerTarget = (ITargetableUI) parameters[0];
    }
    private void ReleaseCurrentPointerTarget(params object[] parameters)
    {
        var slot = (ITargetableUI) parameters[0];

        if (ReferenceEquals(currentPointerTarget, slot))
        {
            currentPointerTarget = null;
        }
    }
    #endregion
    
    
    
    
    
    
    
    // private void SendMovementCommand(Vector3 hitPoint)
    // {
    //     var command = new MovementCommand()
    //         .SetPoint(hitPoint)
    //         .SetForcedExecution(InputManager.ForcePositionKey(GetKeyType.GetKeyAndDown));
    //                 
    //     //OnMovement?.Invoke(command);
    // }
    //
    // private void Cosas(Ray ray)
    // {
    //     if (ExtendedMovement) //while you hold you movement key pressed
    //     {
    //         _raycastCD = raycastCDBase;
    //         
    //         if (Physics.Raycast(ray, out var hit, 100f, LayersUtility.TraversableMask))
    //         {
    //             SendMovementCommand(hit.point);
    //         }
    //         else
    //         {
    //             if (_plane.Plane.Raycast(ray, out var result))
    //             {
    //                 SendMovementCommand(ray.GetPoint(result));
    //             }
    //         }
    //     }
    //     else
    //     {
    //         if (InputManager.CursorMain(GetKeyType.GetKeyDown))
    //         {
    //             _raycastCD = raycastCDBase;
    //             
    //             if (EventSystem.current.IsPointerOverGameObject())
    //             {
    //                 Debug.LogWarning("I'M OVER AN UI ELEMENT");
    //             }
    //             else if (Physics.Raycast(ray, out RaycastHit hit, 100f,
    //                 LayersUtility.CursorSelectorMask))
    //             {
    //                 if (hit.collider.gameObject.layer == LayersUtility.TraversableMaskIndex)
    //                 {
    //
    //                     SendMovementCommand(hit.point);
    //
    //                 }
    //                 else if (hit.collider.gameObject.layer == LayersUtility.EntityMaskIndex)
    //                 {
    //                 
    //                     //OnMovement?.Invoke(hit.point);
    //                 }
    //             }
    //             else
    //             {
    //                 if (_plane.Plane.Raycast(ray, out var result))
    //                 {
    //                     SendMovementCommand(ray.GetPoint(result));
    //                 }
    //             }
    //         }
    //     }
    // }
}
