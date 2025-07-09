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
        float startAngle = -angleRange / 2f;                             // 범위 절반에서 시작
        float angleStep = angleRange / (bulletCount - 1);               // 각 탄환 간 각도 간격

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);     // y축 회전

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation * rotation); // 총알 생성

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                Vector3 direction = (transform.rotation * rotation) * Vector3.forward;
            }
        }

    }
}
