using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float _smoothSpeed = 5f;

    private Vector3 _endPos;
    public bool _canFollow;

    private void FixedUpdate()
    {
        if (!_canFollow) return;
        if (transform.position.y >= _endPos.y) return;
        transform.position = Vector3.Slerp(transform.position,_endPos,_smoothSpeed*Time.fixedDeltaTime);
    }

    public void UpdateCamera()
    {
        _endPos = transform.position;
        _endPos.y = _target.position.y + 1.5f;
    }
}
