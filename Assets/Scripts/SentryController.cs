using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryController : MonoBehaviour
{
    public float angle = 45f;
    public float rotateSpeed = 5f;
    
    private float timeCounter = 0f;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
                                                                                        // Mathf.PingPong���� 0~90�� ���̿��� �պ�
        timeCounter += Time.deltaTime * rotateSpeed;
        float rotationAngle = Mathf.PingPong(timeCounter, angle * 2) - angle;           // -45�� ~ +45��

        Quaternion relativeRotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);  // ���� y�� ȸ��
        Quaternion targetRotation = initialRotation * relativeRotation;                 // �ʱ� ���⿡ ��� ȸ�� ����
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);                              // ���� ���� ǥ��
    }
}
