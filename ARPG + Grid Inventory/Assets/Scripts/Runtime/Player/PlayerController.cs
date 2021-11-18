using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoaT.AI;

public class PlayerController : MonoBehaviour, IUpdate, IAttackable, IGridEntity
{
    [HideInInspector] public string currentState = "";
    private readonly StateManager _stateManager = new StateManager();
    
    private PlayerModel _model;
    private CharacterCommand _queuedCommand = default;
    private CharacterCommand _currentCommand = default;
    

    public int currentIndex = 0; //WIP
    public Path path; //WIP
    
    public bool DebugMe = false;
    public string _queuedCommandSTR;
    public string _currentCommandSTR;

    public AudioClip energySyphonError;
    
    public event Action<IGridEntity> OnMove;

    public PlayerModel Model => _model;
    public Vector3 Position => transform.position;
    public CharacterCommand QueuedCommand
    {
        get => _queuedCommand;
        private set
        {
            if(DebugMe) Debug.Log("Command Assigned to Queue: " + value);
            _queuedCommand = value;
        }
    }
    public CharacterCommand CurrentCommand
    {
        get => _currentCommand;
        private set
        {
            if(DebugMe) Debug.Log("Command Assigned to Current: " + value);
            _currentCommand = value;
        }
    }


    public CursorGameSelection selection;
    public Ability MarkAbility;
    public Ability MarkSwitchAbility;
    public Ability MarkExplode;


    private void Start()
    {
        _model = GetComponent<PlayerModel>();
            
        
        FindObjectOfType<InputCommandManager>().OnCommandCall += OnCommand;

        var map = new Dictionary<Type, State>
        {
            {typeof(PlayerIdle), new PlayerIdle(_stateManager, this)},
            {typeof(PlayerStandUp), new PlayerStandUp(_stateManager, this)},
            {typeof(PlayerInteract), new PlayerInteract(_stateManager, this)},
            {typeof(PlayerUseAbility), new PlayerUseAbility(_stateManager, this)}
        };

        _stateManager.SetStates(map, map[typeof(PlayerStandUp)]);
    
        UpdateManager.AddUpdate(this);
    }

    public void OnUpdate()
    {
        //TEMP
        _model.Mana.AddValue(3 * Time.deltaTime);

        if (DebugMe)
        {
            _queuedCommandSTR = QueuedCommand == null ? "Null" : QueuedCommand.GetType().ToString();
            _currentCommandSTR = CurrentCommand == null ? "Null" : CurrentCommand.GetType().ToString();
        }

        _stateManager.Update();
        
        Rotate(_model.RotationDirectionNormalized);
    }

    private void OnCommand(CharacterCommand command)
    {
        if (command is AbilityCommand aComm)
        {
            if (aComm.Ability == MarkAbility)
            {
                if (selection.raycastResult is EntityRaycastResult entity)
                {
                    AbilityEffectData.MarkAbility?.Invoke(entity.Entity);
                }
                else
                {
                    SoundManager.PlaySound(energySyphonError);
                }

                return;
            }
            
            if (aComm.Ability == MarkSwitchAbility)
            {
                AbilityEffectData.MarkSwitch?.Invoke(null);   
                
                return;
            }

            if (aComm.Ability == MarkExplode)
            {
                AbilityEffectData.MarkExplode?.Invoke(null);
                return;
            }
        }
        
        
        QueuedCommand = command;
        
        if(DebugMe) Debug.Log("Called UpdateQueue from Controller->OnCommand");
        UpdateQueue();
    }

    public void UpdateQueue()
    {
        if (DebugMe)
        {
            Debug.Log("|-----------------------------------------------------------------------------|");
            Debug.Log($"Current Command ID = {(_currentCommand == null ? "Null" : _currentCommand.HashID.ToString())} Before Updating");
            Debug.Log($"Queued Command ID = {(_queuedCommand == null ? "Null" : _queuedCommand.HashID.ToString())} Before Updating");
        }

        if (CurrentCommand == null)
        {
            if (QueuedCommand == null) return;
            
            CurrentCommand = QueuedCommand;
            QueuedCommand = null;
        }
        else if (CurrentCommand.Finished)
        {
            CurrentCommand = QueuedCommand;
            QueuedCommand = null;
        }

        if (DebugMe)
        {
            Debug.Log("|-----------------------------------------------------------------------------|");
            Debug.Log($"Current Command ID = {(_currentCommand == null ? "Null" : _currentCommand.HashID.ToString())} After Updating");
            Debug.Log($"Queued Command ID = {(_queuedCommand == null ? "Null" : _queuedCommand.HashID.ToString())} After Updating");
            Debug.Log("|-----------------------------------------------------------------------------|");
        }
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
        _model.Health.TakeDamage(amount);
    }

    public void InstantiateParticleSystem(ParticleSystem prefab)
    {
        if (prefab == null) return;

        var transform1 = transform;
        var pSys = Instantiate(prefab, _model.RayInitPosition, transform1.rotation * prefab.transform.rotation);
    }

    public void CleanCommands()
    {
        _currentCommand = null;
        _queuedCommand = null;
    }


    public void UpdatePosition()
    {
        OnMove?.Invoke(this);
    }
}
