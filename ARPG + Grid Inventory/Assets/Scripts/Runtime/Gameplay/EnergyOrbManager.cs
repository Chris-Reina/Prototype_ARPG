using System;
using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;

public class EnergyOrbManager : MonoBehaviour
{
    public static EnergyOrbManager Instance { get; private set; }
    
    public IEnergyReceiver target;
    public PlayerModel defaultModel;
    public EnergyType None;
    
    public GameObject feedbackObject;
    public Renderer feedbackRend;
    
    [Range(0f, 1f)] public float lerpMovementSpeed = 0.2f;
    [Range(0f, 1f)] public float lerpSizeSpeed = 0.2f;
    public float smallSize;
    public float bigSize;
    
    public AnimationCurve effectCurve;
    public float effectTime;

    private static readonly int EmissionTint = Shader.PropertyToID("_EmissionTint");
    private static readonly int EmissionIntensity = Shader.PropertyToID("_EmissionIntensity");

    // private float DistanceToTarget => target
    //     ? Vector3.Distance(target.transform.position, feedbackObject.transform.position)
    //     : Vector3.Distance(EnergyHolder.Instance.idleFeedbackOrbPosition.position, feedbackObject.transform.position);
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        feedbackRend = feedbackObject.GetComponentInChildren<Renderer>();
        feedbackRend.material.SetColor(EmissionTint, None.energyColor);
    }

    private void Update()
    {
        if (target == null)
        {
            feedbackObject.transform.position = Vector3.Lerp(feedbackObject.transform.position,
                EnergyHolder.Instance.idleFeedbackOrbPosition.position, lerpMovementSpeed);
            
            var size = Mathf.Lerp(feedbackObject.transform.localScale.x,smallSize, lerpSizeSpeed); 
            feedbackObject.transform.localScale = new Vector3(size, size, size);
        }
        else
        {
            feedbackObject.transform.position = Vector3.Lerp(feedbackObject.transform.position,
                target.FeedbackPosition.position, lerpMovementSpeed);
            
            var size = Mathf.Lerp(feedbackObject.transform.localScale.x,bigSize, lerpSizeSpeed); 
            feedbackObject.transform.localScale = new Vector3(size, size, size);
        }
    }

    public void SetTarget(IEnergyReceiver newTarget)
    {
        if (target != null)
        {
            target.IsTarget = false;
        }

        if (newTarget == null)
        {
            target = null;
            if (feedbackRend)
            {
                feedbackRend.material.SetColor(EmissionTint, EnergyHolder.Instance.CurrentIndexColor);
            }
        }
        if (newTarget is EnergyOrb orb)
        {
            target = orb;
            if (feedbackRend)
            {
                feedbackRend.material.SetColor(EmissionTint, target.EnergyType.energyColor); }
            orb.IsTarget = true;
        }
        else if(newTarget is EnergyDoorReceiver doorReceiver)
        {            
            target = doorReceiver;
            if (feedbackRend)
            {
                feedbackRend.material.SetColor(EmissionTint, EnergyHolder.Instance.CurrentIndexColor);
            }
            doorReceiver.IsTarget = true;
        }
    }

    public void UpdateOrbSocketColorIdle(Color color)
    {
        if (target == null || target is EnergyDoorReceiver)
        {
            feedbackRend.material.SetColor(EmissionTint, color);
        }
    }

    public void ExplodeResponse(bool done)
    {
        if (done)
        {
            SetTarget(null);
        }
    }
}
