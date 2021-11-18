using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public Item droppedItem;
    public Transform sphereTransform;
    
    public float Radius => (transform.localScale.x * sphereTransform.localScale.x) / 2;
    
    private Animator _animator;
    private Material _sphereMaterial;
    private static readonly int Spawn = Animator.StringToHash("Spawn");
    private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

    public bool Interactable => true;
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _sphereMaterial = GetComponentInChildren<Renderer>().material;
    }

    private void Start()
    {
        if (droppedItem != null)
            Initialize(droppedItem);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _animator.SetTrigger(Spawn);
        }
    }

    public void Initialize(Item item)
    {
        droppedItem = item;
        //_sphereMaterial.color = ItemQualityUtility.GetColor(item.quality);
        _sphereMaterial.SetColor(OutlineColor, ItemQualityUtility.GetColor(item.quality));
        _animator.SetTrigger(Spawn);
    }

    private void Dispose()
    {
        Destroy(gameObject);
    }

    public bool Interact()
    {
        EventManager.Trigger(EventsData.OnItemPickUp, droppedItem, new Action(Dispose));
        return true; //WIP
    }
}
