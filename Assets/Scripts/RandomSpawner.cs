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
    private bool isInitialized = false;                                                         // �ʱ�ȭ ���� �߰�

    private List<GameObject> spawnedObjects = new List<GameObject>();                           // ��ȯ�� ��ü�� ����

    void Awake()
    {
        // �ʱ⿡�� ��Ȱ��ȭ ���·� ����
        enabled = false; // ��ũ��Ʈ ��Ȱ��ȭ
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                ClearSpawnedObjects();                                                          // �ð� ���� �� ��ȯ�� ��ü ����
            }

        }
    }

   public void InitializeSpawner()                                                              // �ܺο��� �������� �ʱ�ȭ
    {
        if (!isInitialized)
        {
            isInitialized = true;
            enabled = true;                                                                     // ��ũ��Ʈ Ȱ��ȭ
            StartCoroutine(SpawnRoutine());
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

            yield return new WaitForSeconds(spawnInterval);                                     // 5�� ���

            if (timeElapsed >= totalDuration)                                                   // ���� ����
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
                Destroy(obj);                                                                   // ��ȯ�� ��ü ����
}
        }
        spawnedObjects.Clear();                                                                 // ����Ʈ ����
    }
}
