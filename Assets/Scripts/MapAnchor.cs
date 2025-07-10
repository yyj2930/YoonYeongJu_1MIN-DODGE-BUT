using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnchor : MonoBehaviour
{
    public float mapBoundary = 10f;                                         // �� ��� �ݰ� 
    public LayerMask targetLayer;                                           // ��踦 ������ ���̾�

    void Start()
    {
        
    }

    void FixedUpdate()                                                      // ���� ������Ʈ�� ���� ����
    {
                                                                            // ������ ���̾��� ������Ʈ�� ã�� ��� ���� ����
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
                rb.velocity = Vector3.zero;                                  // Rigidbody�� �ִٸ� �ӵ��� 0���� ������ �ڿ������� ����
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

    // ������ ��� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(mapBoundary * 2, 1, mapBoundary * 2));
    }
}
