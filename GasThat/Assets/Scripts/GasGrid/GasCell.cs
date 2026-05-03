using UnityEngine;

public class GasCell : MonoBehaviour
{
    public int gridX, gridY;
    private GasGrid grid;
    private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Transform planeMesh; // drag the Plane child here
    [SerializeField] private Material claimedMaterial;
    private bool wasClaimedLastFrame = false;

    void Start()
    {
        grid = Object.FindAnyObjectByType<GasGrid>();
        meshRenderer = planeMesh.GetComponent<MeshRenderer>();

        // Only scale the plane, not the whole object
        planeMesh.localScale = new Vector3(grid.cellSize / 10f, 1f, grid.cellSize / 10f);

        if (meshRenderer != null)
            meshRenderer.enabled = false;
    }

    void Update()
    {
        var gas = grid.GetGas(gridX, gridY);
        var team = grid.GetTeam(gridX, gridY);
        
        var main = particle.main;
        main.startColor = team.color;
        var emission = particle.emission;
        emission.rateOverTime = gas > 0.05f ? gas * 20f : 0f;
    
        // Show texture when cell is claimed
        var isClaimed = grid.IsClaimed(gridX, gridY);
        if (isClaimed && meshRenderer != null)
        {
            if (!wasClaimedLastFrame)
            {
                // Apply color to the claimed material
                claimedMaterial.color = team.color;
                meshRenderer.material = team.claimedMaterial;
                meshRenderer.enabled = true;
                wasClaimedLastFrame = true;
            }
        }
        else if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
            wasClaimedLastFrame = false;
        }
    }
}
