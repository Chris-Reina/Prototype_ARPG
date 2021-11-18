using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionHandler : MonoBehaviour
{
    [SerializeField] private GameObject _target = default;

    [SerializeField] private Vector3 displacement = default;
    [Range(0f, 1f), SerializeField] private float cameraLerpRatio = 0.66f;

    private void Awake()
    {
        if (displacement == default)
            displacement = transform.position - _target.transform.position;
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, _target.transform.position + displacement, cameraLerpRatio);
    }
}
