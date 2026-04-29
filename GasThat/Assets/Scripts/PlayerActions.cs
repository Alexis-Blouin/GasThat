using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform look;
    [SerializeField] private LayerMask hitLayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(look.position, look.TransformDirection(Vector3.forward) * 5, Color.yellow);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("Shoot!");
        if (Physics.Raycast(look.position, look.TransformDirection(Vector3.forward) * 100, out var hit, Mathf.Infinity,
                hitLayer))
        {
            Debug.Log("Hit!");
        }
        else
        {
            Debug.Log("No Hit!");
        }
    }
}
