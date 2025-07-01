using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float mouseSensitivity = 70f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float jumpCooldown = 2f;

    [Header("References")]
    public Transform cameraHolder;
    public TMP_InputField chatInputField;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 500f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump = true;
    private bool isJumping = false;

    private CharacterController controller;
    private Animator animator;

    public static bool isChatBoxOpen = false; // 🧠 Lock movement when chatting

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isChatBoxOpen)
            return; // 🚀 No movement if chatbox is active

        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Rotate Player with Mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Movement Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        bool isMoving = move.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && z > 0;

        if (!isJumping)
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("jump", true);
            isJumping = true;
            StartCoroutine(ResetJumpBool());
            StartCoroutine(JumpCooldown());
        }

        // Spell Cast
        if (Input.GetMouseButtonDown(0) && projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * projectileForce);
            animator.SetBool("cast", true);
            StartCoroutine(ResetCastBool());
        }

        if (isJumping) return;

        // Animation Control
        bool isW = Input.GetKey(KeyCode.W);
        bool isA = Input.GetKey(KeyCode.A);
        bool isS = Input.GetKey(KeyCode.S);
        bool isD = Input.GetKey(KeyCode.D);

        ResetAllMovementAnimations();

        if (isS && isA)
            animator.SetBool("back left", true);
        else if (isS && isD)
            animator.SetBool("back right", true);
        else if (isS)
            animator.SetBool("back walk", true);
        else if (isW && isA)
            animator.SetBool("front right", true);
        else if (isW && isD)
            animator.SetBool("front left", true);
        else if (isW)
        {
            if (isRunning)
                animator.SetBool("run", true);
            else
                animator.SetBool("walk", true);
        }
        else if (isA)
            animator.SetBool("left", true);
        else if (isD)
            animator.SetBool("right", true);
        else if (!isMoving)
            animator.SetBool("idle", true);
    }

    private void ResetAllMovementAnimations()
    {
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("idle", false);
        animator.SetBool("front right", false);
        animator.SetBool("front left", false);
        animator.SetBool("back right", false);
        animator.SetBool("back left", false);
        animator.SetBool("back walk", false);
        animator.SetBool("left", false);
        animator.SetBool("right", false);
    }

    private IEnumerator ResetJumpBool()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("jump", false);
        isJumping = false;
    }

    private IEnumerator ResetCastBool()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("cast", false);
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
}
