using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoaT;
using DoaT.AI;
using UnityEngine;

//IA2-P1
public class ArcherController : MonoBehaviour, IUpdate, IEntity, IAttackable, IPath, IGridEntity
{
    public string currentState = "";
    private readonly StateManager _stateManager = new StateManager();
    
    private ArcherModel _model;

    public int CurrentIndex { get; set; }
    public Path Path { get; set; }
    
    public ArcherModel Model => _model;
    public string Name => name;
    public float Durability => _model.health.HealthAmount;
    public Vector3 Position => transform.position;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public bool IsEnemy => true;
    public bool IsDead => _model.IsDead;
    public List<GameObject> Neighbours => GetNeighbours();
    public List<SteeringBehaviour> _steeringBehaviours;
    public float neighbourRadiusDetection = 2f;

    public bool DebugMe;
    public event Action<IGridEntity> OnMove;
    
    
    
    private void Awake()
    {
        _model = GetComponent<ArcherModel>();

        var map = new Dictionary<Type, State>
        {
            {typeof(ArcherIdle), new ArcherIdle(_stateManager, this)},
            {typeof(ArcherDeath), new ArcherDeath(_stateManager, this)},
            {typeof(ArcherMovement), new ArcherMovement(_stateManager, this)},
            {typeof(ArcherAttack), new ArcherAttack(_stateManager, this)}
        };

        _stateManager.SetStates(map, map[typeof(ArcherIdle)]);

        _steeringBehaviours = GetComponents<SteeringBehaviour>().ToList();
    }

    private void Start()
    {
        UpdateManager.AddUpdate(this);
    }
    
    public void OnUpdate()
    {
        if (_model.IsMoving)
            OnMove?.Invoke(this);
        _stateManager.Update();
        
        Rotate(_model.RotationDirectionNormalized);
    }

    public bool IsTargetVisible() //WIP URGENT
    {
        if(Vector3.Distance(_model.targetData.Position,Position) >  _model.data.viewDistance) return false;
        
        var ray = new Ray(_model.RayInitPosition,
            (_model.targetData.Position + new Vector3(0, 0.5f, 0)) - _model.RayInitPosition);

        if (Physics.Raycast(ray, out var hit, _model.data.viewDistance, LayersUtility.PlayerDetectionCheck))
        {
            if (hit.collider.gameObject.layer == LayersUtility.PlayerMaskIndex)
                return true;
        }

        return false;
    }
    
    private void Rotate(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        
        var targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, Vector3.up), Vector3.up);
        

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation,
            Model.RotationSpeedCurve * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        _model.health.TakeDamage(amount * _model.damageTakenMultiplier);
        EventManager.Trigger(EventsData.OnEntityDamageTaken, transform.position, this, amount, false);
    }

    public void Despawn()
    {
        _model.collider.enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        UpdateManager.RemoveUpdate(this);
    }

    private List<GameObject> GetNeighbours()
    {
        return Physics.OverlapSphere(transform.position, neighbourRadiusDetection, LayersUtility.EntityMask,
                QueryTriggerInteraction.Collide)
            .Where(x => x.gameObject != gameObject)
            .Select(x => x.gameObject)
            .ToList();
    }
}
