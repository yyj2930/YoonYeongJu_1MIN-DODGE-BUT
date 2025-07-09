using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanShapedSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCountMax;
    public int bulletCountMin = 3;
    public int bulletCount;
    public float angleRange = 60;
        
    // Start is called before the first frame update
    void Start()
    {
        bulletCount = Random.Range(bulletCountMin, bulletCountMax);
        FireFanBullets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireFanBullets()
    {
        Vector3 spawnPosition = transform.position;
        float startAngle = -angleRange / 2f;                             // ���� ���ݿ��� ����
        float angleStep = angleRange / (bulletCount - 1);               // �� źȯ �� ���� ����

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);     // y�� ȸ��

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation * rotation); // �Ѿ� ����

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                Vector3 direction = (transform.rotation * rotation) * Vector3.forward;
            }
        }

    }
}
