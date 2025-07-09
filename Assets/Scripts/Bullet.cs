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

        bounceNum = Random.Range(bounceMin, bounceMax);                             // ƨ��� Ƚ�� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "BOUNCYBULLET")                                        // ƨ��� Ƚ�� �Ҹ� �� �ڵ� ����
        {
            if(bounceNum < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.tag == "NORMALBULLET" && collision.gameObject.tag == "WALL")  // �Ϲ� źȯ ó��
        {
            if (collision.gameObject.tag == "WALL")
            {
                Destroy(gameObject);
            }
        }
        else if(gameObject.tag == "BOUNCYBULLET" && collision.gameObject.tag == "WALL")     // ƨ��� źȯ ó��
        {
            Vector3 incident = bulletRigidbody.velocity.normalized;                    // ƨ��� ���� ����
            Vector3 normal = collision.contacts[0].normal;                             // �浹 ���� ���� ����
            Vector3 reflect = Vector3.Reflect(incident, normal);                       // ƨ�� ���� ����
                
            bulletRigidbody.velocity = reflect * curSpeed;                             // �ݻ� �������� �ӵ� ����
            bulletRigidbody.MovePosition(transform.position + reflect * Time.fixedDeltaTime * curSpeed); // ���� �̵�
            bounceNum--;

            curSpeed = speed;
            
            /*  ���� �浹 �� Ȯ�ο�
            Vector3 contactPoint = collision.contacts[0].point; 
            Debug.Log($"Incident: {incident}, Normal: {normal}, Reflect: {reflect}");   
            Debug.DrawRay(contactPoint, incident * 5f, Color.blue, 5f);                 // �Ի� ���� (�Ķ�)
            Debug.DrawRay(contactPoint, normal * 5f, Color.red, 5f);                    // ���� (����)
            Debug.DrawRay(contactPoint, reflect * 5f, Color.green, 5f);                 // �ݻ� ���� (�ʷ�)
            */
        }

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Die();
            Destroy(gameObject);
        }
    }
}
