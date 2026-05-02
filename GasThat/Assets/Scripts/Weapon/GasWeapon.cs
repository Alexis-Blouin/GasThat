using UnityEngine;

public class GasWeapon : Weapon
{
    [SerializeField] private float gasDelay = 0.5f;
    
    private Color color;
    
    private float gasTimer = 0;

    private void Start()
    {
        var main = particle.GetComponent<ParticleSystem>().main;
        main.startColor = color;
    }
    
    protected override void Update()
    {
        base.Update();
        
        gasTimer += Time.deltaTime;
    }

    public override void Fire(Transform look)
    {
        base.Fire(look);
        if (gasTimer >= gasDelay)
        {
            Vector3 gasPos = look.position + look.forward * 5f;
            gasPos.y = 0;
            GameManager.Instance.GasGrid.AddGas(gasPos, 0.4f, color, radius: 1);
            gasTimer = 0;
        }
    }

    public void SetColor(Color c)
    {
        color = c;
        var main = particle.GetComponent<ParticleSystem>().main;
        main.startColor = color;
    }
}
