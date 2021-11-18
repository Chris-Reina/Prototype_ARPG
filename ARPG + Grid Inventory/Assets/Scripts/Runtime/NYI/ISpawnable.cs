using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable
{
    void Activate(Vector3 position, Quaternion rotation);
    void Deactivate();
}
