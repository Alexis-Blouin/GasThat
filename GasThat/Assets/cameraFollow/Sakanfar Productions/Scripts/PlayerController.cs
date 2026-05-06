using UnityEngine;
using UnityEngine.InputSystem;


namespace PlayerController // Or any other appropriate namespace
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float gravityStrength = 1;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask = 1;
        [SerializeField] private float groundCheckYOffset = 0.0f;

        [Header("Movement Smoothing")]
        [SerializeField] private float accelerationTime = 0.1f;
        [SerializeField] private float decelerationTime = 0.1f;

        [Header("Input Settings")]
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;

        // Components
        // private CharacterController controller;
        private CapsuleCollider col;
        private Rigidbody rb;

        // Movement variables
        private Vector3 velocity;
        private bool isGrounded;
        private Vector2 currentInputVector;
        private Vector2 smoothInputVelocity;
        private Vector2 targetInputVector;

        // Movement state
        private bool isRunning;
        private float currentSpeed;
        
        // Animation
        private Animator animator;

        void Start()
        {
            // Get required components
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            // Create ground check if it doesn't exist
            if (groundCheck == null)
            {
                GameObject groundCheckObj = new GameObject("GroundCheck");
                groundCheckObj.transform.SetParent(transform);
                groundCheckObj.transform.localPosition = new Vector3(0, groundCheckYOffset / 2, 0);
                
                groundCheck = groundCheckObj.transform;
            }
        }

        void Update()
        {
            HandleGroundCheck();
            HandleInput();
            HandleMovement();
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }

        private void HandleGroundCheck()
        {
            // Check if player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        private void HandleInput()
        {
            // Smooth input for better movement feel
            float smoothTime = targetInputVector.magnitude > 0 ? accelerationTime : decelerationTime;
            currentInputVector = Vector2.SmoothDamp(currentInputVector, targetInputVector, ref smoothInputVelocity, smoothTime);
        }

        private void HandleMovement()
        {
            // Calculate current speed based on running state
            currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Calculate movement direction relative to player rotation
            var moveDirection = transform.right * currentInputVector.x + transform.forward * currentInputVector.y;

            // Apply movement
            velocity.x = moveDirection.x * currentSpeed;
            velocity.z = moveDirection.z * currentSpeed;
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            targetInputVector = context.ReadValue<Vector2>();
            animator.SetBool("IsMoving", targetInputVector != Vector2.zero);
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            isRunning = context.performed;
            animator.SetBool("Run", isRunning);
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }

        // Public methods for external access
        public bool IsGrounded()
        {
            return isGrounded;
        }

        public bool IsRunning()
        {
            return isRunning && currentInputVector.magnitude > 0.1f;
        }

        public bool IsMoving()
        {
            return currentInputVector.magnitude > 0.1f;
        }

        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }

        public void SetMovementSpeeds(float newWalkSpeed, float newRunSpeed)
        {
            walkSpeed = newWalkSpeed;
            runSpeed = newRunSpeed;
        }

        public void SetJumpHeight(float newJumpHeight)
        {
            jumpHeight = newJumpHeight;
        }

        // Gizmos for debugging
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
            }
        }
    }
}