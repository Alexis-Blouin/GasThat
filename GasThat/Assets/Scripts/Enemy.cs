using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Material hitMaterial;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit()
    {
        Debug.Log("Hit!");
        GetComponent<Renderer>().material = hitMaterial;
    }
}
