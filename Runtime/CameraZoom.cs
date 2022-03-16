using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem {

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private float _zoomSpeed = 10f;

    private float _currentZoom = 3;
    private int _targetZoom;

    private int _maxZoom = 10;

    private Vector3 _totalMovement;

    private Vector3 _zoomStepDelta;

    private Vector2 _screenCenter = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);

    public void Init()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(_screenCenter);
        if (Physics.Raycast(ray, out hit, 99999, 1 << LayerMask.NameToLayer("Grid"))) 
        {
            Vector3 zoomDirection = Vector3.Normalize(hit.point - transform.position);
            Vector3 maxZoomPosition = hit.point - zoomDirection;

            Vector3 distanceBetweenCurrentAndMaxZoom = maxZoomPosition - transform.position;
            _zoomStepDelta = distanceBetweenCurrentAndMaxZoom / (_maxZoom - _currentZoom);
        }
        else
        {
            // Debug.LogError("Could not calculate Zoom conditions properly");
            _zoomStepDelta = transform.forward;
        }

        _targetZoom = Mathf.CeilToInt(Mathf.Max(0, Mathf.Min(_currentZoom, _maxZoom)));
        _totalMovement = _zoomStepDelta * _currentZoom;
    }

    public Vector3 GetTotalMovement()
    {
        return _totalMovement;
    }

    public void UpdateZoom(int delta)
    {
        _targetZoom = Mathf.Max(0, Mathf.Min(_targetZoom + delta, _maxZoom));

        if(_currentZoom == _targetZoom)
            return;
            
        float deltaStep = Mathf.Sign(_targetZoom - _currentZoom) * _zoomSpeed * Time.fixedDeltaTime;
        _currentZoom += deltaStep;

        if(Mathf.Abs(_currentZoom - _targetZoom) < deltaStep)
            _currentZoom = _targetZoom;

        _totalMovement = _zoomStepDelta * _currentZoom;
    }
}

}