using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject particle;
    [SerializeField] protected Transform barrelTip;
    [SerializeField] protected float destroyTimer = 5.0f;
    [SerializeField] protected LayerMask hitLayer;

    protected GameObject CurrentParticle;

    protected Team Team;

    protected virtual void Update()
    {
        if (CurrentParticle != null)
        {
            CurrentParticle.transform.position = barrelTip.position;
            CurrentParticle.transform.rotation = barrelTip.rotation;
        }
    }

    public virtual void Fire(Transform look)
    {
        if (CurrentParticle == null)
            CurrentParticle = Instantiate(particle, barrelTip.position, barrelTip.rotation);
    }
    
    public virtual void Release()
    {
        Destroy(CurrentParticle, destroyTimer);
        CurrentParticle.GetComponent<ParticleSystem>().Stop();
        CurrentParticle = null;
    }

    public virtual void SetTeam(Team t)
    {
        Team = t;
    }
}
