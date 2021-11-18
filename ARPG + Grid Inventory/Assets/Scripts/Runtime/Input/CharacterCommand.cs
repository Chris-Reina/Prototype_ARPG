using System;
using UnityEngine;
using  System.Security.Cryptography;
using System.Text;
using Random = UnityEngine.Random;

[Serializable]
public class CharacterCommand
{
    // static string ComputeSha256Hash(string rawData)  
    // {  
    //     // Create a SHA256   
    //     using (SHA256 sha256Hash = SHA256.Create())  
    //     {  
    //         // ComputeHash - returns byte array  
    //         byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
    //
    //         // Convert byte array to a string   
    //         StringBuilder builder = new StringBuilder();
    //         for (int i = 0; i < bytes.Length; i++)  
    //         {  
    //             builder.Append(bytes[i].ToString("x2"));  
    //         }  
    //         return builder.ToString();  
    //     }  
    // }  
    
    
    [SerializeField] protected Vector3 _point;
    [SerializeField] protected bool _forceExecution;

    public Vector3 Point => _point;
    public bool ForceExecution => _forceExecution;

    public int HashID { get; protected set; }

    public bool Initialized { get; protected set; }
    public bool Finished { get; protected set; }

    public virtual void Initialize()
    {
        Initialized = true;
    }

    public virtual void Finish()
    {
        Finished = true;
    }
}

[Serializable]
public class AbilityCommand : CharacterCommand
{
    [SerializeField] private Ability _ability;
    [SerializeField] private Attack _defaultAttack;
    [SerializeField] private MovementAbility _movementAbility;
    [SerializeField] private bool _isMovement = false;
    [SerializeField] private bool _fullMovement = false;

    public bool IsMovement => _isMovement;
    public bool FullMovement => _fullMovement;
    public Ability Ability => _ability;
    public Attack DefaultAbility => _defaultAttack;
    public MovementAbility MovementAbility => _movementAbility;

    public AbilityCommand(Vector3 point, bool forceExecution)
    {
        HashID = Random.Range(int.MinValue + 1, int.MaxValue - 1);
        _point = point;
        _forceExecution = forceExecution;
    }

    public AbilityCommand SetDefaultAttack(Attack attack)
    {
        _defaultAttack = attack;
        return this;
    }
    
    public AbilityCommand SetAbility(Ability ability)
    {
        _ability = ability;
        return this;
    }
    public AbilityCommand SetMovementAbility(MovementAbility ability, bool isMovement, bool fullMovement)
    {
        _isMovement = isMovement;
        _movementAbility = ability;
        _fullMovement = fullMovement;
        return this;
    }

}

[Serializable]
public class InteractCommand : CharacterCommand
{
    private IInteractable _interactable;
    private bool _fullMovement = false; 
    private MovementAbility _movementAbility;

    public bool FullMovement => _fullMovement;
    public IInteractable Interactable => _interactable;
    public MovementAbility MovementAbility => _movementAbility;

    
    public InteractCommand SetInteractable(IInteractable interactable)
    {
        _interactable = interactable;
        return this;
    }
    
    public InteractCommand SetMovementAbility(MovementAbility ability, bool fullMovement)
    {
        _movementAbility = ability;
        _fullMovement = fullMovement;
        return this;
    }

    public InteractCommand(Vector3 point, bool forceExecution)
    {
        HashID = Random.Range(int.MinValue + 1, int.MaxValue - 1);
        _point = point;
        _forceExecution = forceExecution;
    }
}