using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Animator animator;

    public float speed;

    public float dashSpeed;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;
    public bool isDash;
    private Vector3 dashDirection;
    private Collider playerCollider;
    private float lastDashTime;
    public bool isDead;

    public float fallThreshold = -10f;

    private bool isWalking = false;
    private bool isRolling = false;
    private bool isGrounded = false;

    public float rotationSpeed = 10f;

    private Vector3 initialLobbyPosition = new Vector3(0f, 80f, 0f);

    private Vector3 lastPosition;
    private float positionSaveTime;
    public float positionSaveInterval = 5f;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        isDash = false;
        lastDashTime = -dashCooldown;                                         // 초기값 설정
        isDead = false;

        animator = GetComponentInChildren<Animator>();

        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            LoadPosition();
            if (!PlayerPrefs.HasKey("PlayerX"))
            {
                transform.position = initialLobbyPosition;
            }
            lastPosition = transform.position;                                // 초기 위치 저장
            positionSaveTime = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = 0f;
        float zInput = 0f;

        if (Input.GetKey(KeyCode.W)) zInput = 1f;
        else if (Input.GetKey(KeyCode.S)) zInput = -1f;
        if (Input.GetKey(KeyCode.A)) xInput = -1f;
        else if (Input.GetKey(KeyCode.D)) xInput = 1f;

        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        float yVelocity = isGrounded ? 0f : playerRigidbody.velocity.y;
        Vector3 newVelocity = new Vector3 (xSpeed, yVelocity, zSpeed);
        playerRigidbody.velocity = newVelocity;

        if (newVelocity.magnitude > 0f)
        {
            RotateToDirection(newVelocity.normalized);
        }

        isWalking = Mathf.Abs(xInput) > 0f || Mathf.Abs(zInput) > 0f;
        animator.SetBool("Walk_Anim", isWalking);

       if (Input.GetKeyDown(KeyCode.Space) && !isDash && Time.time - lastDashTime >= dashCooldown)
        {
            Dash();
        }

       if (SceneManager.GetActiveScene().name == "Lobby" && newVelocity.magnitude > 0f)
        {
            SavePosition();
            SaveLastPosition();
        }

        CheckFall();
    }

    private void Dash()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        dashDirection = new Vector3(xInput, 0f, zInput).normalized;         // 방향 추출

        if (dashDirection.magnitude == 0)                                   // 별도로 이동 입력이 없을 경우
        {
            dashDirection = transform.forward;
        }

        isDash = true;
        playerCollider.enabled = false;
        isRolling = true;

        lastDashTime = Time.time;                                           // 쿨타임 시작 시각 기록

        animator.SetBool("Roll_Anim", isRolling);

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()                                     // 대쉬 중 상태 관리
    {
        float dashTime = 0f;
        while (dashTime < dashDuration)
        {
            Vector3 dashVelocity = dashDirection * dashSpeed;
            playerRigidbody.velocity = dashVelocity;

            dashTime += Time.deltaTime;
            yield return null;
        }

        isDash = false;
        playerCollider.enabled = true;
        isRolling = false;

        animator.SetBool("Roll_Anim", isRolling);
    }

    private void RotateToDirection(Vector3 direction)
    {
        if (animator != null)
        {
            Transform childTransform = animator.transform;

            Quaternion targetRotation = Quaternion.LookRotation(direction);         // 목표 회전 계산

            childTransform.rotation = Quaternion.Slerp(childTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // 부드러운 회전 적용
        }
    }

    public void Die()
    {
        if (!isDash)
        {
            if(SceneManager.GetActiveScene().name == "Lobby")
            {
                transform.position = lastPosition;  
                isDead = false;
            }
            else
            {
                isDead = true;
                gameObject.SetActive(false);
            }
        }       
    }

    private void SaveLastPosition()
    {
        if (isGrounded && Time.time - positionSaveTime >= positionSaveInterval) 
        {
            lastPosition = transform.position;
            positionSaveTime = Time.time;
        }
    }

    private void SavePosition()
    {
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", transform.position.z);
        PlayerPrefs.Save();
    }

    private void LoadPosition()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");
            transform.position = new Vector3(x, y, z);
        }
    }

    private void CheckFall()
    {
        if (transform.position.y < fallThreshold)
        {
            Die();                                              // y축이 fallThreshold 아래로 떨어지면 추락 처리
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AIR")
        {
            isGrounded = false;
        }
    }
}
