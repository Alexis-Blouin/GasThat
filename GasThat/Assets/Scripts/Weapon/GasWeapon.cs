using UnityEngine;

public class GasWeapon : Weapon
{
    [SerializeField] private float gasDelay = 0.5f;
    [SerializeField] private float baseRechargeRate = 0.2f;

    private float gasTimer = 0;
    private float rechargeTimer = 0;
    private float currentRechargeRate;

    protected override void Start()
    {
        base.Start();
        currentRechargeRate = baseRechargeRate;
    }
    
    protected override void Update()
    {
        if (CurrentParticle != null)
		{
        	base.Update();
        
        	gasTimer += Time.deltaTime;

        	if (currentMagCount == 0)
        	{
            	CurrentParticleSystem.Stop();
        	}
		}
        else
        {
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer >= currentRechargeRate && currentMagCount < magazineCapacity)
            {
                currentMagCount++;
                UpdateUI();
                rechargeTimer = 0;
            }
        }
    }

    public override void Fire(Transform look)
    {
        if (currentMagCount <= 0)
            return;
        
        base.Fire(look);
        if (gasTimer >= gasDelay)
        {
            Vector3 gasPos = look.position + look.forward * 5f;
            gasPos.y = 0;
            GameManager.Instance.GasGrid.AddGas(gasPos, 0.4f, Team, radius: 1);
            gasTimer = 0;
        }
    }

    public override void SetTeam(Team t)
    {
        Team = t;
        particle = t.particle;

        var ps = particle.GetComponent<ParticleSystem>();
        var ren = particle.GetComponent<Renderer>();
        ren.material = Team.gasMaterial;
        var main = ps.main;
        main.startColor = Team.color; // then set the color
    }

    public void SetRechargeRate(float multiplier = 1.0f)
    {
        currentRechargeRate = baseRechargeRate * multiplier;
    }
}
