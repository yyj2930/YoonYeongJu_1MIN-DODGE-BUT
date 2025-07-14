using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject risingObject; 
    public float riseDuration = 1.5f; 
    public Vector3 riseDistance = new Vector3(0f, 10f, 0f); 
    private bool isTransitioning = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning && risingObject != null)
        {
            StartCoroutine(TransitionToLobby());
        }
    }

    private IEnumerator TransitionToLobby()
    {
        isTransitioning = true;

        Vector3 startPosition = risingObject.transform.position;
        Vector3 targetPosition = startPosition + riseDistance;

        float elapsedTime = 0f;
        while (elapsedTime < riseDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / riseDuration;
            risingObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        risingObject.transform.position = targetPosition;

        SceneManager.LoadScene("Lobby");

        isTransitioning = false;
    }
}
