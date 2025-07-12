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
                                                                                        // Mathf.PingPong으로 0~90도 사이에서 왕복
        timeCounter += Time.deltaTime * rotateSpeed;
        float rotationAngle = Mathf.PingPong(timeCounter, angle * 2) - angle;           // -45도 ~ +45도

        Quaternion relativeRotation = Quaternion.AngleAxis(rotationAngle, Vector3.up);  // 로컬 y축 회전
        Quaternion targetRotation = initialRotation * relativeRotation;                 // 초기 방향에 상대 회전 적용
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);                              // 현재 방향 표시
    }
}
