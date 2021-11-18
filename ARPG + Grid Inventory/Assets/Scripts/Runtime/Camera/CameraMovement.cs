using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraMovement : MonoBehaviour
{
    public bool SetUp = true;
    [Range(0f, 1f)] public float cameraDistanceLerp;
    [Range(0.01f, 10f)] public float sensitivity = 1f;
    
    [SerializeField] private GameObject _target;

    private float CameraDistanceLerp
    {
        get => cameraDistanceLerp;
        set => cameraDistanceLerp = Mathf.Clamp01(value);
    }
    private Vector3 Direction
    {
        get
        {
            var temp  = (_target.transform.position - transform.position).normalized;

            if (temp == Vector3.zero)
                temp += transform.forward;

            return temp;
        }
    }

    [Header("Near")]
    [Range(0f, 360f),SerializeField] private float yRotationNear = 45;
    [Range(0f, 90f),SerializeField] private float xRotationNear = 45f;
    [Range(1f, 50f),SerializeField] private float distanceNear = 10;
    
    [Header("Far")]
    [Range(0f, 360f),SerializeField] private float yRotationFar = 45;
    [Range(0f, 90f),SerializeField] private float xRotationFar = 45f;
    [Range(1f, 50f),SerializeField] private float distanceFar = 10;

    private float YRotation => Mathf.Lerp(yRotationNear, yRotationFar, CameraDistanceLerp);
    private float XRotation => Mathf.Lerp(xRotationNear, xRotationFar, CameraDistanceLerp);
    private float Distance => Mathf.Lerp(distanceNear, distanceFar, CameraDistanceLerp);

    private void Awake()
    {
        if(_target == null)
            _target = transform.parent.gameObject;

        cameraDistanceLerp = 1f;
    }

    private void OnEnable()
    {
        if(_target == null)
            _target = transform.parent.gameObject;
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            if(Input.GetAxis("Mouse ScrollWheel") != 0)
                CameraDistanceLerp += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            
        }
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        _target.transform.rotation = Quaternion.Euler(XRotation, YRotation, _target.transform.rotation.eulerAngles.z);
        transform.position = _target.transform.position - (Direction * Distance);
    }
}

