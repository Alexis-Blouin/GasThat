using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform look;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private LayerMask groundLayer;

    private Weapon inHandWeapon;
    [SerializeField] private GameObject[] weapons;
    
    private bool _isFiring = false;
    
    
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
        if (_isFiring)
        {
            inHandWeapon.Fire(look);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isFiring = true;
        }
        else if (context.canceled)
        {
            inHandWeapon.Release();
            _isFiring = false;
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
