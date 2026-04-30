using UnityEngine;

public enum ShootType
{
    Single,
    Burst,
    Hold
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private ShootType shootType;
    [SerializeField] private GameObject particle;
    [SerializeField] private Transform barrelTip;
    [SerializeField] private float destroyTimer = 5.0f;

    private GameObject _currentParticle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentParticle != null)
        {
            _currentParticle.transform.position = barrelTip.position;
            _currentParticle.transform.rotation = barrelTip.rotation;
        }
    }

    public void Shoot()
    {
        // Debug.Log("shoot my gun");
        _currentParticle = Instantiate(particle, barrelTip.position, barrelTip.rotation);
    }

    public void Release()
    {
        if (shootType == ShootType.Hold)
        {
            Destroy(_currentParticle, destroyTimer);
            _currentParticle.GetComponent<ParticleSystem>().Stop();
            _currentParticle = null;
        }
    }
}
