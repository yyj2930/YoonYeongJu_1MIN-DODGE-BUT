using System.Collections;
using System.Collections.Generic;
using UnityEngine.SearchService;
using UnityEngine;

public class StageLoad : MonoBehaviour
{
    public bool isExistPlayer;

    public GameObject stageLoader;
    public GameObject stageCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        isExistPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isExistPlayer == true)
        {
            stageCanvas.SetActive(true);
        }
        else if(isExistPlayer == false)
        {
            stageCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isExistPlayer = true;
            stageLoader.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isExistPlayer = false;
            stageLoader.SetActive(false);
        }
    }    
}
