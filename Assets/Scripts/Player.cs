using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    private float lashDashTime;
    public bool isDead;

    private bool isWalking = false;
    private bool isRolling = false;

    public float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        isDash = false;
        lashDashTime = -dashCooldown;                                         // �ʱⰪ ����
        isDead = false;

        animator = GetComponentInChildren<Animator>();
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

        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidbody.velocity = newVelocity;

        if (newVelocity.magnitude > 0f)
        {
            RotateToDirection(newVelocity.normalized);
        }

        isWalking = Mathf.Abs(xInput) > 0f || Mathf.Abs(zInput) > 0f;
        animator.SetBool("Walk_Anim", isWalking);

       if (Input.GetKeyDown(KeyCode.Space) && !isDash && Time.time - lashDashTime >= dashCooldown)
        {
            Dash();
        }
    }

    private void Dash()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        dashDirection = new Vector3(xInput, 0f, zInput).normalized;         // ���� ����

        if (dashDirection.magnitude == 0)                                   // ������ �̵� �Է��� ���� ���
        {
            dashDirection = transform.forward;
        }

        isDash = true;
        playerCollider.enabled = false;
        isRolling = true;

        lashDashTime = Time.time;                                           // ��Ÿ�� ���� �ð� ���

        animator.SetBool("Roll_Anim", isRolling);

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()                                     // �뽬 �� ���� ����
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

            Quaternion targetRotation = Quaternion.LookRotation(direction);         // ��ǥ ȸ�� ���

            childTransform.rotation = Quaternion.Slerp(childTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // �ε巯�� ȸ�� ����
        }
    }

    public void Die()
    {
        if (!isDash)
        {
            isDead = true;
            gameObject.SetActive(false);
        }       
    }
}
