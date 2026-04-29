// using UnityEngine;
// using UnityEngine.InputSystem;
//
// public class PlayerController : MonoBehaviour
// {
//     [SerializeField] private float walkSpeed;
//     [SerializeField] private float runSpeed;
//     [SerializeField] private float jumpForce;
//     
//     private float _speed;
//     private Vector3 _direction;
//     private Rigidbody _rb;
//     
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         _speed = walkSpeed;
//         _rb = GetComponent<Rigidbody>();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         
//     }
//
//     private void FixedUpdate()
//     {
//         _rb.linearVelocity = new Vector3(
//             _direction.x * _speed,
//             _rb.linearVelocity.y,
//             _direction.z * _speed
//         );
//     }
//
//     public void OnMove(InputAction.CallbackContext context)
//     {
//         var input = context.ReadValue<Vector2>();
//         _direction = new Vector3(input.x, 0f, input.y);
//     }
//
//     public void OnJump(InputAction.CallbackContext context)
//     {
//         if (context.performed)
//             _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//     }
// }
