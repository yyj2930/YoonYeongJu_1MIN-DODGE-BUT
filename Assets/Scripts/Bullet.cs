using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private float curSpeed;
    private Rigidbody bulletRigidbody;

    public float bounceMax = 5;
    public float bounceMin = 1;

    private float bounceNum;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = speed * transform.forward;
        curSpeed = speed;

        bounceNum = Random.Range(bounceMin, bounceMax);                             // 튕기는 횟수 랜덤
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "BOUNCYBULLET")                                        // 튕기는 횟수 소모 시 자동 제거
        {
            if(bounceNum < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.tag == "NORMALBULLET" && collision.gameObject.tag == "WALL")  // 일반 탄환 처리
        {
            if (collision.gameObject.tag == "WALL")
            {
                Destroy(gameObject);
            }
        }
        else if(gameObject.tag == "BOUNCYBULLET" && collision.gameObject.tag == "WALL")     // 튕기는 탄환 처리
        {
            Vector3 incident = bulletRigidbody.velocity.normalized;                    // 튕기기 전의 벡터
            Vector3 normal = collision.contacts[0].normal;                             // 충돌 지점 법선 벡터
            Vector3 reflect = Vector3.Reflect(incident, normal);                       // 튕긴 후의 벡터
                
            bulletRigidbody.velocity = reflect * curSpeed;                             // 반사 방향으로 속도 설정
            bulletRigidbody.MovePosition(transform.position + reflect * Time.fixedDeltaTime * curSpeed); // 강제 이동
            bounceNum--;

            curSpeed = speed;
            
            /*  벽과 충돌 시 확인용
            Vector3 contactPoint = collision.contacts[0].point; 
            Debug.Log($"Incident: {incident}, Normal: {normal}, Reflect: {reflect}");   
            Debug.DrawRay(contactPoint, incident * 5f, Color.blue, 5f);                 // 입사 방향 (파랑)
            Debug.DrawRay(contactPoint, normal * 5f, Color.red, 5f);                    // 법선 (빨강)
            Debug.DrawRay(contactPoint, reflect * 5f, Color.green, 5f);                 // 반사 방향 (초록)
            */
        }

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Die();
            Destroy(gameObject);
        }
    }
}
