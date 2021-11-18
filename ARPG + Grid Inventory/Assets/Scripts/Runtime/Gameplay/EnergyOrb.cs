using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class EnergyOrb : MonoBehaviour, IEnergyReceiver, IEnergySensitive
{
    public Model model;
    public EnergyType energyType;
    public Renderer rend;
    public Transform feedbackPosition;

    public Transform FeedbackPosition
    {
        get => feedbackPosition;
        set => feedbackPosition = value;
    }

    public bool IsTarget { get; set; }
    public EnergyType EnergyType 
    {
        get => energyType;
        set => energyType = value;
    }
    public bool IsAffected { get; set; }
    

    private static readonly int EmissionTint = Shader.PropertyToID("_EmissionTint");

    private void Awake()
    {
        model = GetComponent<Model>();
    }

    private void Start()
    {
        model.OnDeath += TryGiveEnergy;

        if (rend)
        {
            rend.material.SetColor(EmissionTint, energyType.energyColor);
        }
    }

    private void TryGiveEnergy()
    {
        if (EnergyOrbManager.Instance.target != this) return;
        
        EnergyOrbManager.Instance.SetTarget(null);
        EnergyHolder.Instance.GiveEnergy(energyType);
    }

    public bool Explode(EnergyType type)
    {
        if (IsAffected) return false;

        switch (type.name)
        {
            case "Lightning":
                model.AffectLightning();
                IsAffected = true;
                break;
            case "Fire":
                model.AffectFire();
                IsAffected = true;
                break;
            case "Cold":
                model.AffectCold();
                IsAffected = true;
                break;
        }

        return IsAffected;
    }
    
}
