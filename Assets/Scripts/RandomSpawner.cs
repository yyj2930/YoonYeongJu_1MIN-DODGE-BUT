using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] patterns;
    public float spawnInterval = 5f;
    public float totalDuration = 60f;

    private float timeElapsed = 0f;
    private bool isSpawning = false;

    private List<GameObject> spawnedObjects = new List<GameObject>();                           // 소환된 객체를 추적

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());    
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= totalDuration)
            {
                isSpawning = false;
                ClearSpawnedObjects();                                                          // 시간 종료 시 소환된 객체 제거
            }

        }
    }

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;
        timeElapsed = 0f;

        while (isSpawning)
        {
            if (patterns.Length > 0)
            {
                int randomIndex = Random.Range(0, patterns.Length);
                GameObject patternToSpawn = patterns[randomIndex];

                Instantiate(patternToSpawn, transform.position, transform.rotation);
            }

            yield return new WaitForSeconds(spawnInterval);                                     // 5초 대기

            if (timeElapsed >= totalDuration)                                                   // 루프 종료
            {
                break;
            }
        }
    }

    private void ClearSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);                                                                   // 소환된 객체 제거
}
        }
        spawnedObjects.Clear();                                                                 // 리스트 비우기
    }
}
