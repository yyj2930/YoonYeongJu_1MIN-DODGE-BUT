using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private float curSpeed;
    private Rigidbody bulletRigidbody;

    public float bounceMax = 5;
    public float bounceMin = 1;

    private float bounceNum;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = speed * transform.forward;
        curSpeed = speed;

        bounceNum = Random.Range(bounceMin, bounceMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "BOUNCYBULLET")
        {
            if(bounceNum < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "NORMALBULLET")
        {
            if (other.tag == "WALL")
            {
                Destroy(gameObject);
            }
        }
        if(gameObject.tag == "BOUNCYBULLET")
        {
            if(other.tag == "WALL" && curSpeed >= 0)
            {
                bulletRigidbody.velocity = -speed * transform.forward;
                curSpeed = -speed;
                bounceNum--;
            }
            else if(other.tag == "WALL" && curSpeed <= 0)
            {
                bulletRigidbody.velocity = speed * transform.forward;
                curSpeed = speed;
                bounceNum--;
            }
        }

        if(gameObject.tag == "Player")
        {
            other.GetComponent<Player>().Die();
            Destroy(gameObject);
        }
    }
}
