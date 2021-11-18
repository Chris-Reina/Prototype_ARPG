using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnergyMonitor : MonoBehaviour
{
    public EnergyHolder holder;

    public GameObject firstOverlay;
    public GameObject secondOverlay;
    public GameObject thirdOverlay;
    
    public Image firstEnergyImage;
    public Image secondEnergyImage;
    public Image thirdEnergyImage;

    public Image currentEnergyImage;
    public RectTransform rotationPivot;
    
    [Range(0f, 1f)] public float lerpStrength = 0.33f;

    private const float FirstRotationZ = 0;
    private const float SecondRotationZ = -120;
    private const float ThirdRotationZ = -240;

    private void Awake()
    {
        if(!holder) holder = FindObjectOfType<EnergyHolder>();
    }

    private void Start()
    {
        if(!holder) holder = EnergyHolder.Instance;
    }

    private void Update()
    {
        UpdateRotation();
        UpdateOverlay();
        UpdateColors();
    }

    private void UpdateColors()
    {
        firstEnergyImage.color = holder.firstEnergy.energyColor;
        secondEnergyImage.color = holder.secondEnergy.energyColor;
        thirdEnergyImage.color = holder.thirdEnergy.energyColor;
        
        Color curCol;
        
        // var curCol = EnergyOrbManager.Instance.target
        //     ? EnergyOrbManager.Instance.target.energyType.energyColor
        //     : holder.None.energyColor;

        if (EnergyOrbManager.Instance.target == null || EnergyOrbManager.Instance.target is EnergyDoorReceiver)
            curCol = holder.None.energyColor;
        else
            curCol = EnergyOrbManager.Instance.target.EnergyType.energyColor;
        

        curCol *= 0.7f;
        curCol.a = 1f;
        currentEnergyImage.color = curCol;
    }

    private void UpdateRotation()
    {
        var rotTarget = rotationPivot.rotation;
        var eulerRot = rotationPivot.rotation.eulerAngles;
        switch (holder.CurrentIndexEnum)
        {
            case EnergyHolder.EnergyNodeActive.FirstNode:
                eulerRot.z = FirstRotationZ;
                rotTarget = Quaternion.Euler(eulerRot);
                break;
            case EnergyHolder.EnergyNodeActive.SecondNode:
                eulerRot.z = SecondRotationZ;
                rotTarget = Quaternion.Euler(eulerRot);
                break;
            case EnergyHolder.EnergyNodeActive.ThirdNode:
                eulerRot.z = ThirdRotationZ;
                rotTarget = Quaternion.Euler(eulerRot);
                break;
            case EnergyHolder.EnergyNodeActive.All:
                eulerRot.z += 10;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        rotationPivot.rotation = Quaternion.Lerp(rotationPivot.rotation, rotTarget, lerpStrength);
    }

    private void UpdateOverlay()
    {
        firstOverlay.SetActive(holder.currentEnergyIndex == 0);
        secondOverlay.SetActive(holder.currentEnergyIndex == 1);
        thirdOverlay.SetActive(holder.currentEnergyIndex == 2);
    }
}
