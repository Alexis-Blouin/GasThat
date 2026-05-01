using UnityEngine;

public class BulletWeapon : Weapon
{
    // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    public override void Fire(Transform look)
    {
        base.Fire(look);
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
