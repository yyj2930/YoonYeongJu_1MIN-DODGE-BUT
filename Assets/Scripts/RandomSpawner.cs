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

                Instantiate(patternToSpawn, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);                                     // 5초 대기

            if (timeElapsed >= totalDuration)                                                   // 루프 종료
            {
                break;
            }
        }
    }
}
