using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem {

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CameraMovement _cameraMovement;

    [SerializeField]
    private CameraZoom _cameraZoom;

    private Vector3 _initialPosition;

    void Start()
    {
        _cameraZoom.Init();
        _initialPosition = transform.position - _cameraZoom.GetTotalMovement();
    }
    
    public void UpdateCamera(Vector2 cursorPosition, float scrollDelta)
    {
        UpdateCameraPosition(cursorPosition);
        UpdateCameraZoom(scrollDelta);

        transform.position = _initialPosition + _cameraMovement.GetTotalMovement() + _cameraZoom.GetTotalMovement();
    }

    private void UpdateCameraPosition(Vector2 mousePosition)
    {
        _cameraMovement.UpdateMovement(mousePosition);
    }

    private void UpdateCameraZoom(float delta)
    {
        _cameraZoom.UpdateZoom(Mathf.RoundToInt(delta));
    }
}

}