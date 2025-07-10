using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnchor : MonoBehaviour
{
    public float mapBoundary = 10f;                                         // 맵 경계 반경 
    public LayerMask targetLayer;                                           // 경계를 적용할 레이어

    void Start()
    {
        
    }

    void FixedUpdate()                                                      // 물리 업데이트에 맞춰 실행
    {
                                                                            // 지정된 레이어의 오브젝트를 찾아 경계 내로 조정
        Collider[] colliders = Physics.OverlapSphere(transform.position, mapBoundary * 2f, targetLayer);
        foreach (Collider collider in colliders)
        {
            AdjustPosition(collider.transform);
        }
    }

    private void AdjustPosition(Transform target)
    {
        Vector3 clampedPosition = ClampToBoundary(target.position);
        if (target.position != clampedPosition)
        {
            target.position = clampedPosition;
            Rigidbody rb = target.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;                                  // Rigidbody가 있다면 속도를 0으로 설정해 자연스럽게 멈춤
            }
        }
    }

    private Vector3 ClampToBoundary(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, -mapBoundary, mapBoundary),
            position.y,
            Mathf.Clamp(position.z, -mapBoundary, mapBoundary)
        );
    }

    // 디버깅용 경계 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(mapBoundary * 2, 1, mapBoundary * 2));
    }
}
