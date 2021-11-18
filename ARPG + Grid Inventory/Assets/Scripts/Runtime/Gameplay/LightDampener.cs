using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDampener : MonoBehaviour, IUpdate
{
    public new Light light;
    public bool dampen = true;

    public float distance = 50f;

    private PlayerModel _target;
    private void Awake()
    {
        light = GetComponent<Light>();
        _target = FindObjectOfType<PlayerModel>();
    }

    private void Start()
    {
        UpdateManager.Instance.AddUpdateToManager(this);
    }

    public void OnUpdate()
    {
        if (!dampen)
        {
            light.enabled = true;
            return;
        }
        
        var a = _target.Position;
        var b = transform.position;

        a.y = 0;
        b.y = 0;

        light.enabled = (Vector3.Distance(a, b) < distance);
    }
}
