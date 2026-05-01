using UnityEngine;

public class GasWeapon : Weapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    public override void Shoot(Transform look, GasGrid gasGrid)
    {
        base.Shoot(look, gasGrid);
        if (Physics.Raycast(look.position, look.TransformDirection(Vector3.forward), out var groundHit, Mathf.Infinity,
                hitLayer))
        {
            Debug.Log("Hit ground!");
            gasGrid.AddGas(groundHit.point, 0.4f, radius: 1);
        }
    }
}
