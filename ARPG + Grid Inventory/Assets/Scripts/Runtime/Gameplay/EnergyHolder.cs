using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHolder : MonoBehaviour
{
    public static EnergyHolder Instance { get; private set; }
    
    public EnergyType firstEnergy;
    public EnergyType secondEnergy;
    public EnergyType thirdEnergy;
    public EnergyType None;

    public Transform idleFeedbackOrbPosition;

    public AudioClip OnSuccessfulExplode;
    public AudioClip OnFailedExplode;
    
    public enum EnergyNodeActive
    {
        FirstNode = 0,
        SecondNode = 1,
        ThirdNode = 2,
        All = 3
    }

    public Tuple<EnergyType, EnergyType, EnergyType> CurrentEnergy =>
        new Tuple<EnergyType, EnergyType, EnergyType>(firstEnergy, secondEnergy, thirdEnergy);
        
    [Range(0, 2)] public int currentEnergyIndex;

    public Dictionary<int, EnergyType> numberToEnergyMap = new Dictionary<int, EnergyType>();
    public Color CurrentIndexColor => numberToEnergyMap[currentEnergyIndex].energyColor;
    public EnergyType CurrentIndexEnergy => numberToEnergyMap[currentEnergyIndex];
    public EnergyNodeActive CurrentIndexEnum => (EnergyNodeActive) currentEnergyIndex;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        numberToEnergyMap.Add(0, firstEnergy);
        numberToEnergyMap.Add(1, secondEnergy);
        numberToEnergyMap.Add(2, thirdEnergy);
    }

    public void ExplodeResponse(bool done)
    {
        if(done)
            GiveEnergy(None);
        
        SoundManager.PlaySound(OnSuccessfulExplode);
        

        // else
        //    SoundManager.PlaySound(OnFailedExplode);
    }
    
    public void GiveEnergy(EnergyType energyType)
    {
        numberToEnergyMap[currentEnergyIndex] = energyType;
        switch (currentEnergyIndex)
        {
            case 0:
                firstEnergy = energyType;
                break;
            case 1:
                secondEnergy = energyType;
                break;
            case 2:
                thirdEnergy = energyType;
                break;
        }
    }

    public bool TrySpendEnergies(Tuple<EnergyType,EnergyType,EnergyType> energyToMatch)
    {
        if (HasCorrectEnergies(energyToMatch))
        {
            ResetEnergies();
            return true;
        }

        return false;
    }

    public void Remove(EnergyType type)
    {
        if (firstEnergy == type)
            firstEnergy = None;
        else if (secondEnergy == type)
            secondEnergy = None;
        else if (thirdEnergy == type)
            thirdEnergy = None;
    }
    
    public void NextEnergySocket()
    {
        if (currentEnergyIndex + 1 > 2)
            currentEnergyIndex = 0;
        else
            currentEnergyIndex += 1;

        EnergyOrbManager.Instance.UpdateOrbSocketColorIdle(numberToEnergyMap[currentEnergyIndex].energyColor);
    }

    public bool TrySpendEnergy(EnergyType type)
    {
        return HasCorrectEnergy(type);
    }
    
    private bool HasCorrectEnergies(Tuple<EnergyType,EnergyType,EnergyType> energyToMatch)
    {
        var list = new List<EnergyType> {firstEnergy, secondEnergy, thirdEnergy};

        var (item1, item2, item3) = energyToMatch;
        for (var i = 2; i >= 0; i--)
        {
            if(list[i] == item1 || list[i] == item2 || list[i] == item3)
                list.RemoveAt(i);
        }

        return list.Count == 0;
    }

    private void ResetEnergies()
    {
        firstEnergy = None;
        secondEnergy = None;
        thirdEnergy = None;

        currentEnergyIndex = 0;
    }

    private bool HasCorrectEnergy(EnergyType type)
    {
        if (firstEnergy == type)
        {
            firstEnergy = None;
            currentEnergyIndex = 0;
            return true; 
        }
        
        if (secondEnergy == type)
        {
            secondEnergy = None;
            currentEnergyIndex = 1;
            return true; 
        }
        
        if (thirdEnergy == type)
        {
            thirdEnergy = None;
            currentEnergyIndex = 2;
            return true; 
        }

        return false;
    }
}
