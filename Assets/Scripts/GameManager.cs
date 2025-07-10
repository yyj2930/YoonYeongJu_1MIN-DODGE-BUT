using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    private float time;
    public Text timeText;

    public GameObject timer;
    public GameObject clearText;
    public GameObject failText;

    // Start is called before the first frame update
    void Start()
    {
        time = 60f;    
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeText.text = "" + (int)time;

        Player player = gameObject.GetComponent<Player>();

        if (time <= 0f)
        {
            clearText.SetActive(true);
            timer.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Lobby");
            }
        }

        if (player.isDead == true)
        {
            failText.SetActive(true);
            timer.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Stage1");
            }
        }
    }
}
