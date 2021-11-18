using System.Collections;
using System.Collections.Generic;
using DoaT.Attributes;
using UnityEngine;

public interface IEntity : IPositionProperty
{
    string Name { get; }
    float Durability { get; }
    Transform Transform { get; }
    GameObject GameObject { get; }
    bool IsEnemy { get; }
    bool IsDead { get; }
}
