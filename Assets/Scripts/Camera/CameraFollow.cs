using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target; // за кем следим
    [SerializeField] private float smoothSpeed = 5f; // плавность

    private void LateUpdate()// LateUpdate выполняется после всех остальных обновлений, когда перс занял место
    {
        if (!_target)
            return;
        
        Vector3 targetPosition = _target.position;
        targetPosition.z = transform.position.z;
        
        transform.position = Vector3.Lerp(
            transform.position, 
            targetPosition, 
            smoothSpeed * Time.deltaTime);
    }
}
