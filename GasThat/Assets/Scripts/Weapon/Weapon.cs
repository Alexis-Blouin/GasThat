using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject particle;
    [SerializeField] protected Transform barrelTip;
    [SerializeField] protected float destroyTimer = 5.0f;
    [SerializeField] protected LayerMask hitLayer;

    protected GameObject _currentParticle;

    // Update is called once per frame
    void Update()
    {
        if (_currentParticle != null)
        {
            _currentParticle.transform.position = barrelTip.position;
            _currentParticle.transform.rotation = barrelTip.rotation;
        }
    }

    public virtual void Shoot(Transform look, GasGrid gasGrid = null)
    {
        _currentParticle = Instantiate(particle, barrelTip.position, barrelTip.rotation);
    }
    
    public virtual void Release()
    {
        Destroy(_currentParticle, destroyTimer);
        _currentParticle.GetComponent<ParticleSystem>().Stop();
        _currentParticle = null;
    }
}
