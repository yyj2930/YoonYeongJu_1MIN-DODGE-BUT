using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSpawnBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float rotateSpeed = 30f;
    public float spawnRate = 0.5f;
    private float nextSpawnTime;
    private int bulletCount;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time;                                       // 초기 스폰 시간 설정
        bulletCount = Random.Range(1, 5);                           // 1~4개의 랜덤한 총알 개수
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);         // 오브젝트 회전

        if (Time.time >= nextSpawnTime)
        {
            SpawnBullet();
            nextSpawnTime = Time.time + spawnRate;                      // 다음 스폰 시간 갱신
        }
    }

    void SpawnBullet()
    {
        

        float angleStep = 0f;                                           // bulletCount에 따른 간격 계산
        if (bulletCount == 1) angleStep = 0f;
        else if (bulletCount == 2) angleStep = 180f;
        else if (bulletCount == 3) angleStep = 120f;
        else if (bulletCount == 4) angleStep = 90f;

        float startAngle = -((bulletCount - 1) * angleStep) / 2f;       // 시작 각도

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 spawnDirection = transform.rotation * (rotation * Vector3.forward); // 스포너의 현재 회전을 적용한 방향 계산

            Vector3 spawnPosition = transform.position;
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);

            bullet.transform.forward = spawnDirection.normalized;                       // 총알의 초기 방향을 spawnDirection으로 설정
        }
    }
}
