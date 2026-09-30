using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Movement")]
    public float walkSpeed = 8f;
    public float sprintSpeed = 15f;
    public float acceleration = 25f;
    public float deceleration = 20f;

    [Header("Jump")]
    public float jumpHeight = 2.5f;
    public float gravity = -30f;

    [Header("Air Control")]
    public float airControl = 0.5f;

    [Header("Dash")]
    public float dashSpeed = 30f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1f;

    [Header("Slide")]
    public float slideSpeed = 18f;
    public float slideDuration = 0.7f;
    public float slideHeight = 1f;

    private CharacterController controller;

    private Vector3 horizontalVelocity;
    private Vector3 verticalVelocity;

    private bool isDashing;
    private bool isSliding;

    private float dashTimer;
    private float slideTimer;

    private float normalHeight;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        normalHeight = controller.height;

        dashTimer = dashCooldown;
    }

    void Update()
    {
        UpdateTimers();

        HandleMovement();

        HandleJump();

        HandleDash();

        HandleSlide();

        ApplyGravity();

        MovePlayer();
    }

    void UpdateTimers()
    {
        if (dashTimer < dashCooldown)
            dashTimer += Time.deltaTime;

        if (isDashing)
        {
            dashTimer = 0f;
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }
    }

    void HandleMovement()
    {
        if (isDashing)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection =
            transform.right * x +
            transform.forward * z;

        inputDirection = inputDirection.normalized;

        bool sprinting =
            Input.GetKey(KeyCode.LeftShift);

        float targetSpeed =
            sprinting ? sprintSpeed : walkSpeed;

        if (isSliding)
        {
            targetSpeed = slideSpeed;
        }

        Vector3 targetVelocity =
            inputDirection * targetSpeed;

        float control =
            controller.isGrounded
                ? acceleration
                : acceleration * airControl;

        if (inputDirection.magnitude > 0.1f)
        {
            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                targetVelocity,
                control * Time.deltaTime
            );
        }
        else if (!isSliding)
        {
            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                Vector3.zero,
                deceleration * Time.deltaTime
            );
        }
    }

    void HandleJump()
    {
        if (!controller.isGrounded)
            return;

        if (Input.GetButtonDown("Jump"))
        {
            verticalVelocity.y =
                Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleDash()
    {
        if (!Input.GetKeyDown(KeyCode.Q))
            return;

        if (isDashing)
            return;

        if (dashTimer < dashCooldown)
            return;

        StartDash();
    }

    void StartDash()
    {
        isDashing = true;

        Vector3 dashDirection =
            GetDashDirection();

        horizontalVelocity =
            dashDirection * dashSpeed;

        Invoke(nameof(StopDash), dashDuration);
    }

    Vector3 GetDashDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction =
            transform.right * x +
            transform.forward * z;

        if (direction.magnitude < 0.1f)
        {
            direction = transform.forward;
        }

        return direction.normalized;
    }

    void StopDash()
    {
        isDashing = false;
    }

    void HandleSlide()
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl))
            return;

        if (!controller.isGrounded)
            return;

        if (isSliding)
            return;

        StartSlide();
    }

    void StartSlide()
    {
        isSliding = true;

        slideTimer = slideDuration;

        controller.height = slideHeight;

        Vector3 forward =
            transform.forward;

        horizontalVelocity =
            forward * slideSpeed;
    }

    void StopSlide()
    {
        isSliding = false;

        controller.height = normalHeight;
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y +=
            gravity * Time.deltaTime;
    }

    void MovePlayer()
    {
        Vector3 movement =
            horizontalVelocity +
            verticalVelocity;

        controller.Move(
            movement * Time.deltaTime
        );
    }
}