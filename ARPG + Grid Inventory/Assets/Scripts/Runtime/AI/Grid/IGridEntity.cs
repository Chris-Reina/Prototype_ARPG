using System;
using UnityEngine;

public interface IGridEntity : IPositionProperty
{
    event Action<IGridEntity> OnMove;
}