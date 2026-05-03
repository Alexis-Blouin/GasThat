using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text territoryText;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void UpdateAmmo(int current, int max)
    {
        ammoText.text = $"{current} / {max}";
    }

    public void UpdateTerritory(float territory)
    {
        territoryText.text = $"{territory}%";
    }
}