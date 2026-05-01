using UnityEngine;

public class GasCell : MonoBehaviour
{
    public int gridX, gridY;
    private GasGrid grid;
    private ParticleSystem ps;

    void Start()
    {
        grid = Object.FindAnyObjectByType<GasGrid>();
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        float gas = grid.GetGas(gridX, gridY);
        var color = grid.GetCellColor(gridX, gridY);
        var main = ps.main;
        main.startColor = color;
        var emission = ps.emission;
        emission.rateOverTime = gas > 0.05f ? gas * 20f : 0f;
    }
}
