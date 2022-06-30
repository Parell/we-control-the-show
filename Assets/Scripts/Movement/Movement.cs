using UnityEngine;
using Cinemachine;

public class Movement : MonoBehaviour
{
    public bool turnDisabled = false;
    public bool moveDisabled = false;
    public float moveDisabledTimer = 0;

    [Space]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float moveAcceleration = 10f;
    [SerializeField] float airMoveAcceleration = 5f;
    [SerializeField] float turnAcceleration = 10f;
    [SerializeField] Vector3 currentMovement;

    [Space]
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float groundedGravity = -1f;

    [Space]
    [SerializeField] float jumpHeight = 0.35f;
    [SerializeField] float maxFallSpeed = -20f;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isJumpPressed = false;

    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float coyoteTimeCounter;

    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float jumpBufferTimeCounter;

    [Space]
    [SerializeField] bool isDashPressed = false;
    [SerializeField] float dashLength = 0.1f;
    [SerializeField] float dashSpeed = 5f;
    [SerializeField] float dashResetTime = 1f;

    Vector3 dashMove;
    float dashing = 0f;
    float dashingTime = 0f;

    bool canDash = true;
    bool isDashing = false;
    bool dashReset = true;

    Vector3 moveDirection;
    Vector3 goalVelocity;
    Vector3 velocity;

    Vector3 goalDirection;
    Vector3 direction;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        CinemachineCore.GetInputAxis = HandleCamera;
    }

    void Update()
    {
        //HandleRotation();
        //HandleAnimation();
        HandleInputs();

        characterController.Move(currentMovement * Time.deltaTime);

        HandleMove();
        HandleGravity();
        HandleJump();
        HandleDash();
    }

    float HandleCamera(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (!turnDisabled)
            {
                return Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (!turnDisabled)
            {
                return Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return Input.GetAxis(axisName);
    }

    void HandleInputs()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (moveDisabled || moveDisabledTimer > 0f)
        {
            isJumpPressed = false;
            moveDirection = Vector3.zero;
            moveDisabledTimer -= Time.deltaTime;
        }

        isDashPressed = Input.GetButton("Dash");

        if (Input.GetButton("Jump"))
        {
            isJumpPressed = true;
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }
    }

    void HandleMove()
    {
        goalVelocity = moveDirection * moveSpeed;

        if (characterController.isGrounded)
        {
            velocity = Vector3.MoveTowards(velocity, goalVelocity, Time.deltaTime * moveAcceleration);
        }
        else
        {
            velocity = Vector3.MoveTowards(velocity, goalVelocity, Time.deltaTime * airMoveAcceleration);
        }

        Transform camera = Camera.main.transform;

        goalDirection = camera.forward * velocity.z + camera.right * velocity.x;

        float goalDirectionLength = goalDirection.magnitude;
        goalDirection.y = 0;
        goalDirection = goalDirection.normalized * goalDirectionLength;

        if (goalDirection != Vector3.zero)
        {
            direction = Vector3.Slerp(direction, goalDirection, Time.deltaTime * turnAcceleration);

            transform.rotation = Quaternion.LookRotation(direction);
            characterController.Move(direction * Time.deltaTime);

            //animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }

    void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0f;
        float fallMultiplier = 2f;

        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;

            coyoteTimeCounter = coyoteTime;
        }
        else if (isFalling)
        {
            float lastYVelocity = currentMovement.y;
            float nextYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float newYVelocity = Mathf.Max((lastYVelocity + nextYVelocity) / 2, maxFallSpeed);
            currentMovement.y = newYVelocity;

            coyoteTimeCounter -= Time.deltaTime;
        }
        else
        {
            float lastYVelocity = currentMovement.y;
            float nextYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float newYVelocity = (lastYVelocity + nextYVelocity) / 2;
            currentMovement.y = newYVelocity;

            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void HandleJump()
    {
        characterController.slopeLimit = characterController.isGrounded ? 45 : 90f;

        float jumpVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);

        if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f)
        {
            isJumping = true;
            jumpBufferTimeCounter = 0f;

            currentMovement.y = jumpVelocity;
        }
        else if (!isJumpPressed && characterController.isGrounded)
        {
            isJumping = false;

            coyoteTimeCounter = 0f;
        }
    }

    void HandleDash()
    {
        if (isDashPressed && dashing < dashLength && dashingTime < dashResetTime && dashReset == true && canDash == true)
        {
            dashMove = goalDirection;
            canDash = false;
            dashReset = false;
            isDashing = true;
        }

        if (isDashing && dashing < dashLength)
        {
            characterController.Move(dashMove * dashSpeed * Time.deltaTime);
            dashing += Time.deltaTime;
        }

        if (dashing >= dashLength)
        {
            isDashing = false;
        }

        if (dashReset == false)
        {
            dashingTime += Time.deltaTime;
        }

        if (characterController.isGrounded && canDash == false && dashing >= dashLength)
        {
            canDash = true;
            dashing = 0f;
        }

        if (dashingTime >= dashResetTime && dashReset == false)
        {
            dashReset = true;
            dashingTime = 0f;
        }
    }
}
