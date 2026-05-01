using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform look;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private LayerMask groundLayer;

    private Weapon inHandWeapon;
    [SerializeField] private GameObject[] weapons;
    
    [SerializeField] private GasGrid gasGrid;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (weapons.Length >= 1)
        {
            inHandWeapon = weapons[0].GetComponent<Weapon>();
            weapons[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(look.position, look.TransformDirection(Vector3.forward) * 5, Color.yellow);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Shoot!");
            inHandWeapon.Shoot();
            if (Physics.Raycast(look.position, look.TransformDirection(Vector3.forward), out var hit, Mathf.Infinity,
                    hitLayer))
            {
                Debug.Log("Hit player/enemy!");
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.GetHit();
                }
            }
            else if (Physics.Raycast(look.position, look.TransformDirection(Vector3.forward), out var groundHit, Mathf.Infinity,
                         groundLayer))
            {
                Debug.Log("Hit ground!");
                gasGrid.AddGas(groundHit.point, 0.4f, radius: 1);
            }
            else
            {
                Debug.Log("No Hit!");
            }
        }
        else if (context.canceled)
        {
            inHandWeapon.Release();
        }
    }

    public void OnWeaponSwitch(InputAction.CallbackContext context)
    {
        if (context.performed && weapons.Length >= 2)
        {
            if (inHandWeapon == weapons[0].GetComponent<Weapon>())
            {
                inHandWeapon = weapons[1].GetComponent<Weapon>();
                weapons[0].SetActive(false);
                weapons[1].SetActive(true);
            }
            else
            {
                inHandWeapon = weapons[0].GetComponent<Weapon>();
                weapons[0].SetActive(true);
                weapons[1].SetActive(false);
            }
        }
    }
}
