using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManger : MonoBehaviour
{
    private float time;
    public bool isClear = false;
    public Text timeText;

    public GameObject timer;
    public GameObject clearText;
    public GameObject failText;

    private Player player;

    public float startDelay = 5f;                                              // ���� ���� �ð� (��)
    public List<GameObject> excludedObjects;                                   // ��Ȱ��ȭ���� ������ ������Ʈ ����Ʈ
    public Text startText;                                                      // ǥ���� �ؽ�Ʈ UI
    public string[] displayMessages = { "1MIN Dodge", "But...", "" };          // ǥ���� �޽��� �迭
    public float displayInterval = 1f;                                         // �޽��� ���� (��)

    private List<GameObject> deactivatedObjects = new List<GameObject>();      // ��Ȱ��ȭ�� ������Ʈ ����
    private bool isPaused = true;                                              // �ð� ���� ���� �÷��� (�ʱⰪ true)

    public string currentSceneName;

    void Awake()
    {
        
    }

    void Start()
    {
        isClear = false;
        time = 60f;

        player = GameObject.FindObjectOfType<Player>();

        DeactivateObjects();                                                    // ���� �� ������Ʈ�� ��Ȱ��ȭ
        StartCoroutine(InitializeGame());                                       // ���� �ʱ�ȭ �ڷ�ƾ ����
    }

    void Update()
    {
        if (!isPaused)                                                           // �ð� ���� ���°� �ƴ� ���� �ð� ����
        {
            time -= Time.deltaTime;
            timeText.text = "" + (int)time;
        }

        if (time <= 0f && player.isDead == false)
        {
            isClear = true;
            clearText.SetActive(true);
            timer.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Lobby");
            }
        }
        else if (time <= 0f && isClear == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Lobby");
            }
        }

        if (player.isDead && player != null && isClear == false)
        {
            failText.SetActive(true);
            timer.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(currentSceneName);
            }
        }
    }

    private void DeactivateObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (!excludedObjects.Contains(obj) && obj.activeInHierarchy)
            {
                obj.SetActive(false);
                deactivatedObjects.Add(obj);
            }
        }
    }

    private IEnumerator InitializeGame()
    {
        StartCoroutine(DisplayMessages()); // �޽��� ǥ�� �ڷ�ƾ ����
        yield return new WaitForSeconds(startDelay + (displayMessages.Length * displayInterval)); // ��ü ���� ���

        foreach (GameObject obj in deactivatedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        deactivatedObjects.Clear();

        ObjectRiseAndActivate[] riseObjects = FindObjectsOfType<ObjectRiseAndActivate>();
        foreach (ObjectRiseAndActivate riseObj in riseObjects)
        {
            if (riseObj != null && !riseObj.isRising) // �̹� �۵� ���� �ƴ� ���
            {
                riseObj.StartCoroutine(riseObj.RiseAndActivateRoutine());
                Debug.Log($"Started ObjectRiseAndActivate on {riseObj.gameObject.name}");
            }
        }

        deactivatedObjects.Clear();
        isPaused = false; // �ð� �簳
        time = 60f; // �ð� �ʱ�ȭ
        timer.SetActive(true); // Ÿ�̸� Ȱ��ȭ
    }

    private IEnumerator DisplayMessages()
    {
        startText.gameObject.SetActive(true);

        for (int i = 0; i < displayMessages.Length; i++)
        {
            startText.text = displayMessages[i];
            yield return new WaitForSeconds(displayInterval);
        }

        startText.gameObject.SetActive(false);
    }
}
