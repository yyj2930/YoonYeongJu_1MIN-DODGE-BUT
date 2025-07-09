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
        nextSpawnTime = Time.time;                                       // �ʱ� ���� �ð� ����
        bulletCount = Random.Range(1, 5);                           // 1~4���� ������ �Ѿ� ����
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);         // ������Ʈ ȸ��

        if (Time.time >= nextSpawnTime)
        {
            SpawnBullet();
            nextSpawnTime = Time.time + spawnRate;                      // ���� ���� �ð� ����
        }
    }

    void SpawnBullet()
    {
        

        float angleStep = 0f;                                           // bulletCount�� ���� ���� ���
        if (bulletCount == 1) angleStep = 0f;
        else if (bulletCount == 2) angleStep = 180f;
        else if (bulletCount == 3) angleStep = 120f;
        else if (bulletCount == 4) angleStep = 90f;

        float startAngle = -((bulletCount - 1) * angleStep) / 2f;       // ���� ����

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 spawnDirection = transform.rotation * (rotation * Vector3.forward); // �������� ���� ȸ���� ������ ���� ���

            Vector3 spawnPosition = transform.position;
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);

            bullet.transform.forward = spawnDirection.normalized;                       // �Ѿ��� �ʱ� ������ spawnDirection���� ����
        }
    }
}
