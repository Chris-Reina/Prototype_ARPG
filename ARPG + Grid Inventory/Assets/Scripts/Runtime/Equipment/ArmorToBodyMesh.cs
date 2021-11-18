using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorToBodyMesh : MonoBehaviour
{
    public SkinnedMeshRenderer armorRend;
    public SkinnedMeshRenderer bodyRend;

    private void Awake()
    {
        armorRend = GetComponent<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        armorRend.bones = bodyRend.bones;
    }
}
