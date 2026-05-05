using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Transform look;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private LayerMask groundLayer;

    private Weapon inHandWeapon;
    [SerializeField] private GameObject[] weapons;
    
    private bool isFiring = false;

    [SerializeField] private Team team;
    
    private bool wasInOwnTerritory = false;
    
    private GameManager gameManager;

    [Header("ClaimedZoneModifiers")] [SerializeField]
    private float gasRechargeRateMultiplier = 0.5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        
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
        if (isFiring)
        {
            inHandWeapon.Fire(look);
        }
        
        // Check if player is in own territory
        bool isInOwnTerritory = gameManager.GasGrid.IsPlayerInOwnTerritory(transform.position, team);
        
        if (isInOwnTerritory && !wasInOwnTerritory)
        {
            OnEnteredOwnTerritory();
        }
        else if (!isInOwnTerritory && wasInOwnTerritory)
        {
            OnLeftOwnTerritory();
        }
        
        wasInOwnTerritory = isInOwnTerritory;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFiring = true;
        }
        else if (context.canceled)
        {
            inHandWeapon.Release();
            isFiring = false;
        }
    }

    public void OnWeaponSwitch(InputAction.CallbackContext context)
    {
        if (context.performed && weapons.Length >= 2)
        {
            if (inHandWeapon == weapons[0].GetComponent<Weapon>())
            {
                inHandWeapon = weapons[1].GetComponent<Weapon>();
                inHandWeapon.SetTeam(team);
                weapons[0].SetActive(false);
                weapons[1].SetActive(true);
            }
            else
            {
                inHandWeapon = weapons[0].GetComponent<Weapon>();
                weapons[0].SetActive(true);
                weapons[1].SetActive(false);
            }

            if(inHandWeapon.didStart)
                inHandWeapon.UpdateUI();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        inHandWeapon.Reload();
    }
    
    /// <summary>
    /// Called when player enters their own team's claimed territory
    /// Add your effects here: healing, speed boost, damage bonus, etc.
    /// </summary>
    private void OnEnteredOwnTerritory()
    {
        Debug.Log("Entered own territory! Trigger effects here.");
        if(inHandWeapon is GasWeapon gasWeapon)
        {
            gasWeapon.SetRechargeRate(gasRechargeRateMultiplier);
        }
        // TODO: Add your territory bonus effects here
        // Example ideas:
        // - Apply healing over time
        // - Increase movement speed
        // - Increase weapon damage
        // - Play audio/visual feedback
    }
    
    /// <summary>
    /// Called when player leaves their own team's claimed territory
    /// Remove or deactivate effects here
    /// </summary>
    private void OnLeftOwnTerritory()
    {
        Debug.Log("Left own territory! Remove effects here.");
        if(inHandWeapon is GasWeapon gasWeapon)
        {
            gasWeapon.SetRechargeRate();
        }
        // TODO: Deactivate territory bonus effects
    }
}
