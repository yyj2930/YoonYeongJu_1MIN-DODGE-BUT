using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRiseAndActivate : MonoBehaviour
{
    public float startDelay = 1f;
    public float riseHeigt = 3f;
    public float riseSpeed = 2f;
    public bool activateChildren = true;
    public List<Transform> startActiveChildren;

    private Vector3 initialPosition;
    public bool isRising = false;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        DeactivateChildren();
        PreserveActiveChildren();
        StartCoroutine(RiseAndActivateRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RiseAndActivateRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (transform.position.y < initialPosition.y + riseHeigt)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, initialPosition.y + riseHeigt, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, initialPosition.y + riseHeigt, transform.position.z);

        if(activateChildren)
        {
            ActivateChildren();

            // RandomSpawner 초기화
            RandomSpawner spawner = GetComponentInChildren<RandomSpawner>();
            if (spawner != null)
            {
                spawner.InitializeSpawner();
            }
        }

        isRising = false;
    }

    private void ActivateChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            MonoBehaviour[] scripts = child.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
        }
    }

    private void DeactivateChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);                                           // 자식 오브젝트 비활성화
            MonoBehaviour[] scripts = child.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;                                                  // 자식 스크립트 비활성화
            }
        }
    }

    private void PreserveActiveChildren()
    {
        foreach (Transform child in startActiveChildren)
        {
            if (child != null && child.parent == transform)                                 // 유효한 자식만 처리
            {
                child.gameObject.SetActive(true);                                            // 지정된 자식은 활성화 유지
                MonoBehaviour[] scripts = child.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    script.enabled = true;
                }
            }
        }
    }
}
