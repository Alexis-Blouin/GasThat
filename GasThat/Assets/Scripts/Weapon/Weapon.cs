using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject particle;
    [SerializeField] protected Transform barrelTip;
    [SerializeField] protected float destroyTimer = 5.0f;
    [SerializeField] protected LayerMask hitLayer;

    [SerializeField] protected int magazineCapacity;
    [SerializeField] protected int currentMagCount;
    [SerializeField] protected int totalAmmoCount;
    [SerializeField] protected float fireRate = 0.5f;
    
    protected float nextFire;

    protected GameObject CurrentParticle;
    protected ParticleSystem CurrentParticleSystem;

    protected Team Team;
    
    protected UIManager UIManager;

    protected virtual void Awake()
    {
        currentMagCount = magazineCapacity;
    }

    protected virtual void Start()
    {
        UIManager = UIManager.Instance;
        UpdateUI();
    }
    protected virtual void Update()
    {
        if (CurrentParticle != null)
        {
            CurrentParticle.transform.position = barrelTip.position;
            CurrentParticle.transform.rotation = barrelTip.rotation;
            if (nextFire < Time.time && currentMagCount > 0)
            {
                if (!CurrentParticleSystem.isPlaying)
                    CurrentParticleSystem.Play();
                nextFire = Time.time + fireRate;
                currentMagCount--;
                UpdateUI();
            }
        }
    }

    public virtual void Fire(Transform look)
    {
        if (CurrentParticle == null && nextFire < Time.time)
        {
            CurrentParticle = Instantiate(particle, barrelTip.position, barrelTip.rotation);
            CurrentParticleSystem = CurrentParticle.GetComponent<ParticleSystem>();
            // nextFire = Time.time + fireRate;
        }
    }
    
    public virtual void Release()
    {
        if(CurrentParticle != null)
        {
            CurrentParticleSystem.Stop();
            Destroy(CurrentParticle, destroyTimer);
            CurrentParticle = null;
        }
    }

    public virtual void Reload()
    {
        // TODO use totalAmmoCount as well
        currentMagCount = magazineCapacity;
        UpdateUI();
    }

    public virtual void SetTeam(Team t)
    {
        Team = t;
    }

    public virtual void UpdateUI()
    {
        UIManager.UpdateAmmo(currentMagCount, magazineCapacity);
    }
}
