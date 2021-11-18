
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Model : MonoBehaviour
{
    public Action OnDeath;
    
    protected void CheckComponent<T>(ref T obj)
    {
        if (obj == null)
            obj = GetComponent<T>();
    }
    
    protected void TryFindObjectOfType<T>(ref T obj) where T : Object
    {
        if (obj == null)
            obj = FindObjectOfType<T>();
    }

    public virtual void AffectCold() { }

    public virtual void AffectFire() { }

    public virtual void AffectLightning() { }
}
