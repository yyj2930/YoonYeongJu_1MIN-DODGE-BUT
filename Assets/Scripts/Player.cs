using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody playerRigidbody;

    public float speed;

    private bool immunity;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        immunity = false;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidbody.velocity = newVelocity;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash(speed);
        }


    }

    private void Dash(float speed)
    {
        immunity = true;
    }

    public void Die()
    {
        if (immunity == false)
        {
            gameObject.SetActive(false);
        }
        if (immunity == true)
        {
            return;
        }
        
    }
}
