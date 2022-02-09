using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem {

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float _borderWidthLeft = 100;
    
    [SerializeField]
    private float _borderWidthTop = 50;
    
    [SerializeField]
    private float _borderWidthRight = 100;
    
    [SerializeField]
    private float _borderWidthBottom = 50;

    [SerializeField]
    private float _moveSpeed = 30;

    private Vector3 _totalMovement;

    private Vector2 _screenCenter = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
    private Vector2 _screenQuarterSize = new Vector2 (Screen.width * 0.25f, Screen.height * 0.25f);

    public Vector3 GetTotalMovement() 
    {
        return _totalMovement;
    }

    public void UpdateMovement(Vector2 mousePosition)
    {
        Vector3 offset = new Vector3(0, 0, 0);
        float distanceFactor = 0f;
        
        if(mousePosition.x < _borderWidthLeft )
        {
            offset.x -= 1;
            distanceFactor = (_borderWidthLeft - mousePosition.x) / _borderWidthLeft;
        }
        else if(mousePosition.x > Screen.width - _borderWidthRight)
        {
            offset.x += 1;
            distanceFactor = (mousePosition.x - Screen.width + _borderWidthRight) / _borderWidthRight;
        }

        if(mousePosition.y < _borderWidthBottom)
        {
            offset.z -= 1;
            distanceFactor = (_borderWidthBottom - mousePosition.y) / _borderWidthBottom;
        }
        else if(mousePosition.y > Screen.height - _borderWidthTop)
        {
            offset.z += 1;
            distanceFactor = (mousePosition.y - Screen.height + _borderWidthTop) / _borderWidthTop;
        }

        distanceFactor = Mathf.Max(0, Mathf.Min(distanceFactor, 1));
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(_screenCenter + new Vector2(offset.x, 0) * _screenQuarterSize);
        
        if (!Physics.Raycast(ray, out hit, 99999, 1 << LayerMask.NameToLayer("Grid"))) 
        {
            offset.x = 0;
        }

        ray = Camera.main.ScreenPointToRay(_screenCenter + new Vector2(0, offset.z) * _screenQuarterSize);
        if (!Physics.Raycast(ray, out hit, 99999, 1 << LayerMask.NameToLayer("Grid"))) 
        {
            offset.z = 0;
        }

        if(offset.x == 0 && offset.z == 0) 
            return;

        offset = Vector3.Normalize(offset);
        offset *= distanceFactor * distanceFactor * distanceFactor * distanceFactor * _moveSpeed * Time.fixedDeltaTime;

        _totalMovement += offset;
    }
}

}