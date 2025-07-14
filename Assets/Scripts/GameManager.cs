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

    public float startDelay = 5f;                                              // 시작 지연 시간 (초)
    public List<GameObject> excludedObjects;                                   // 비활성화에서 제외할 오브젝트 리스트
    public Text startText;                                                      // 표시할 텍스트 UI
    public string[] displayMessages = { "1MIN Dodge", "But...", "" };          // 표시할 메시지 배열
    public float displayInterval = 1f;                                         // 메시지 간격 (초)

    private List<GameObject> deactivatedObjects = new List<GameObject>();      // 비활성화된 오브젝트 추적
    private bool isPaused = true;                                              // 시간 멈춤 상태 플래그 (초기값 true)

    public string currentSceneName;

    void Awake()
    {
        
    }

    void Start()
    {
        isClear = false;
        time = 60f;

        player = GameObject.FindObjectOfType<Player>();

        DeactivateObjects();                                                    // 시작 시 오브젝트들 비활성화
        StartCoroutine(InitializeGame());                                       // 게임 초기화 코루틴 시작
    }

    void Update()
    {
        if (!isPaused)                                                           // 시간 멈춤 상태가 아닐 때만 시간 감소
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
        StartCoroutine(DisplayMessages()); // 메시지 표시 코루틴 시작
        yield return new WaitForSeconds(startDelay + (displayMessages.Length * displayInterval)); // 전체 지연 대기

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
            if (riseObj != null && !riseObj.isRising) // 이미 작동 중이 아닌 경우
            {
                riseObj.StartCoroutine(riseObj.RiseAndActivateRoutine());
                Debug.Log($"Started ObjectRiseAndActivate on {riseObj.gameObject.name}");
            }
        }

        deactivatedObjects.Clear();
        isPaused = false; // 시간 재개
        time = 60f; // 시간 초기화
        timer.SetActive(true); // 타이머 활성화
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
