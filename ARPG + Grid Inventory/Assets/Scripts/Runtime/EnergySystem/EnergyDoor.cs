using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnergyDoor : MonoBehaviour
{
    public List<EnergyType> EnergyTypeToOpen;
    [ColorUsage(true, true)] public List<Color> ColorByIndex;
    public Vector3 OpenRotation;
    [SerializeField] private bool _isOpen = false;

    [SerializeField] private Collider physicalCollider = default;
    

    public Vector3 Position => transform.position;
    public string Name => name;
    public float Durability => 1f;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public bool IsEnemy => true;
    public bool IsDead => _isOpen;

    public EnergyType None;
    
    [SerializeField] private Renderer _rend;
    [SerializeField] private AudioClip OpenSuccessAudio = default;
    [SerializeField] private AudioClip OpenFailureAudio = default;

    private void Awake()
    {
        if(!_rend) _rend = GetComponent<Renderer>();
    }

    private void Start()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        for (var i = 0; i < EnergyTypeToOpen.Count; i++)
        {
            ColorByIndex.Add(EnergyTypeToOpen[i].energyColor);
        }

        if(EnergyTypeToOpen.Count == 0)
            Debug.LogWarning("No energy to Open Door");
        
        for (int i = 0; i < EnergyTypeToOpen.Count; i++)
        {
            var temp = "_EmissionTint";
            temp += i + 1;
            
            _rend.material.SetColor(temp, ColorByIndex[i]);
        }
    }

    private bool TryOpenDoor()
    {
        var list = new List<EnergyType>(EnergyTypeToOpen);
        
        foreach (var type in EnergyTypeToOpen)
        {
            if (EnergyHolder.Instance.firstEnergy != EnergyHolder.Instance.None)
            {
                if (type == EnergyHolder.Instance.firstEnergy)
                {
                    list.Remove(type);
                    continue;
                }
            }
            if(EnergyHolder.Instance.secondEnergy != EnergyHolder.Instance.None)
            {
                if (type == EnergyHolder.Instance.secondEnergy)
                {
                    list.Remove(type);
                    continue;
                }
            }
            if(EnergyHolder.Instance.thirdEnergy != EnergyHolder.Instance.None)
            {
                if (type == EnergyHolder.Instance.thirdEnergy)
                {
                    list.Remove(type);
                }
            }
        }

        if (list.Count == 0)
        {
            transform.rotation *= Quaternion.Euler(OpenRotation);
            var count = EnergyTypeToOpen.Count;
            _isOpen = true;

            for (int i = 0; i < count; i++)
            {
                EnergyHolder.Instance.Remove(EnergyTypeToOpen[i]);
                EnergyTypeToOpen[i] = EnergyHolder.Instance.None;
            }
            UpdateColor();
            return true;
        }
        
        return false;
    }


    public bool CanOpenDoor(EnergyType type)
    {
        Debug.Log("Given type: " + type.energyName);
        Debug.Log("CanOpenDoor" + (!_isOpen && EnergyTypeToOpen.Contains(type)));
        return !_isOpen && EnergyTypeToOpen.Contains(type);
    }
    
    public bool OpenDoor(EnergyType energy)
    {
        if (_isOpen) return true;

        for (int i = 0; i < EnergyTypeToOpen.Count; i++)
        {
            if (EnergyTypeToOpen[i] == energy)
            {
                EnergyTypeToOpen[i] = None;
            }
        }
        
        UpdateColor();

        if (EnergyTypeToOpen.Any(x => x != None)) return false;
        
        transform.rotation *= Quaternion.Euler(OpenRotation);
        _isOpen = true;
        physicalCollider.enabled = false;
        SoundManager.PlaySound(OpenSuccessAudio,transform.position,0.5f,Tuple.Create(0.2f,1.2f));
        return true;
    }

    private void TakeDamage(float amount)
    {
        if (TryOpenDoor())
        {
            physicalCollider.enabled = false;
            SoundManager.PlaySound(OpenSuccessAudio,transform.position,0.5f,Tuple.Create(0.2f,1.2f));
        }
        else
            SoundManager.PlaySound(OpenFailureAudio,transform.position,1f,Tuple.Create(0.1f,1.5f));
        
    }
}
