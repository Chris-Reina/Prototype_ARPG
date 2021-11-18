using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Plane plane;

    public Plane Plane => plane;
    
    private void Awake()
    {
        if (player == null)
            player = FindObjectOfType<PlayerModel>().gameObject;
        
        plane = new Plane(Vector3.up, player.transform.position);
    }

    public void UpdatePlanePosition()
    {
        plane.SetNormalAndPosition(Vector3.up, player.transform.position);
    }
}
