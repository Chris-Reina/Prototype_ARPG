
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public abstract void Initialize();
    public abstract void Initialize(float initialSpeed);
    public abstract void Initialize(Vector3 directionalSpeed);
    public abstract void Initialize(Vector3 direction, float speed);

    public abstract float GetSpeed();
}
