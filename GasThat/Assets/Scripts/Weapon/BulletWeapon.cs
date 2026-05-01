using UnityEngine;

public class BulletWeapon : Weapon
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
        if (Physics.Raycast(look.position, look.TransformDirection(Vector3.forward), out var hit, Mathf.Infinity,
                hitLayer))
        {
            Debug.Log("Hit player/enemy!");
            if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.GetHit();
            }
        }
    }
}
