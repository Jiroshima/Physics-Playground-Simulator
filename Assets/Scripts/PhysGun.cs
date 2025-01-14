using System;
using System.Collections.Generic;
using UnityEngine;

public class PhysGun : MonoBehaviour
{
    [SerializeField]
    private float _maxGrabDistance = 40f;
    [SerializeField]
    private float _minGrabDistance = 1f;
    [SerializeField]
    private LineRenderer _pickLine;
    [SerializeField]
    private Transform _barrelPoint;

    private Rigidbody _grabbedObject;
    private float _pickDistance;
    private Vector3 _pickOffset;
    private Vector3 _pickTargetPosition;
    private Vector3 _pickForce;

    private void Start()
    {
        if (!_barrelPoint)
        {
            _barrelPoint = transform;
        }

        if (!_pickLine)
        {
            var obj = new GameObject("PhysGun Pick Line");
            _pickLine = obj.AddComponent<LineRenderer>();
            _pickLine.startWidth = 0.02f;
            _pickLine.endWidth = 0.02f;
            _pickLine.useWorldSpace = true;
            _pickLine.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Grab();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (_grabbedObject)
            {
                Release(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_grabbedObject)
            {
                Release();
            }
        }

        _pickDistance = Mathf.Clamp(_pickDistance + Input.mouseScrollDelta.y, _minGrabDistance, _maxGrabDistance);
    }

    private void LateUpdate()
    {
        if (_grabbedObject)
        {
            var midpoint = (transform.position + _pickTargetPosition) / 2f;
            midpoint += Vector3.ClampMagnitude(_pickForce / 2f, 1f);
            DrawQuadraticBezierCurve(_pickLine, _barrelPoint.position, midpoint, _grabbedObject.worldCenterOfMass - _pickOffset);
        }
    }

    private void FixedUpdate()
    {
        if (_grabbedObject != null)
        {
            var ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            _pickTargetPosition = (ray.origin + ray.direction * _pickDistance) + _pickOffset;
            var forceDir = _pickTargetPosition - _grabbedObject.position;
            _pickForce = forceDir / Time.fixedDeltaTime * 0.3f / _grabbedObject.mass;
            _grabbedObject.linearVelocity = _pickForce; // Corrected property
        }
    }

    private void Grab()
    {
        var ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        if (Physics.Raycast(ray, out RaycastHit hit, _maxGrabDistance, ~0) && hit.rigidbody != null)
        {
            _pickOffset = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
            _pickDistance = hit.distance;
            _grabbedObject = hit.rigidbody;
            _grabbedObject.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _grabbedObject.useGravity = false;
            _grabbedObject.freezeRotation = true;
            _grabbedObject.isKinematic = false;
            _pickLine.gameObject.SetActive(true);
        }
    }

    private void Release(bool freeze = false)
    {
        if (_grabbedObject == null) return;

        if (freeze)
        {
            _grabbedObject.linearVelocity = Vector3.zero;
            _grabbedObject.angularVelocity = Vector3.zero;
            _grabbedObject.isKinematic = true; // Set to kinematic to freeze it
        }
        else
        {
            _grabbedObject.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _grabbedObject.useGravity = true;
            _grabbedObject.freezeRotation = false;
            _grabbedObject.isKinematic = false;
        }

        _pickLine.gameObject.SetActive(false);
        _grabbedObject = null; // Clear the reference
    }


    void DrawQuadraticBezierCurve(LineRenderer line, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        line.positionCount = 20;
        for (int i = 0; i < line.positionCount; i++)
        {
            float t = i / (float)(line.positionCount - 1);
            Vector3 B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            line.SetPosition(i, B);
        }
    }
}
