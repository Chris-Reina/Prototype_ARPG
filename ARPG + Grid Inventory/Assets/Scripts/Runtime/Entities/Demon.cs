using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : IEntity
{
    public string Name { get; }
    public float Durability { get; }
    public Vector3 Position { get; }
    public Transform Transform { get; }
    public GameObject GameObject { get; }
    public bool IsEnemy { get; }
    public bool IsDead { get; }
}
