using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GasGrid : MonoBehaviour
{
    public int width = 100;
    public int height = 70;
    public float cellSize = 2f;
    public Vector3 originPosition;

    public float dissipationRate = 0.02f;
    public float spreadInterval = 0.5f;
    private float spreadTimer;

    // Grid data
    private float[,] gasValues;
    private Team[,] cellTeams;
    private bool[,] claimedCells;
    private Team emptyTeam;

    // Visuals — one particle system per cell, managed here
    private ParticleSystem[,] particles;
    private MeshRenderer[,] meshRenderers;

    // Track which cells actually changed this tick
    private HashSet<Vector2Int> dirtyGasCells = new();
    private HashSet<Vector2Int> dirtyClaimedCells = new();

    private Dictionary<Team, int> teamCounts = new();
    private UIManager uiManager;

    [Header("Prefab / Materials")]
    public GameObject gasCellPrefab;

    void Start()
    {
        gasValues   = new float[width, height];
        cellTeams   = new Team[width, height];
        claimedCells = new bool[width, height];
        particles   = new ParticleSystem[width, height];
        meshRenderers = new MeshRenderer[width, height];

        emptyTeam = ScriptableObject.CreateInstance<Team>();
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                cellTeams[i, j] = emptyTeam;

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 pos = GridToWorld(x, y);
            GameObject cell = Instantiate(gasCellPrefab, pos, Quaternion.identity, transform);

            var gasCell = cell.GetComponent<GasCell>();
            gasCell.gridX = x;
            gasCell.gridY = y;

            // Cache component references here instead of per-cell
            particles[x, y]     = cell.GetComponentInChildren<ParticleSystem>();
            var emission = particles[x, y].emission;
            emission.rateOverTime = 0f; // start with no emission
            meshRenderers[x, y] = cell.GetComponentInChildren<MeshRenderer>();
            gasCell.planeMesh.localScale = new Vector3(cellSize / 5f, 1f, cellSize / 5f);

            if (meshRenderers[x, y] != null)
                meshRenderers[x, y].enabled = false;
        }

        uiManager = UIManager.Instance;
    }

    void Update()
    {
        spreadTimer += Time.deltaTime;
        if (spreadTimer >= spreadInterval)
        {
            spreadTimer = 0f;
            Dissipate();
            FlushVisuals(); // only update cells that actually changed
        }
    }

    private void Dissipate()
    {
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (gasValues[x, y] <= 0) continue;

            gasValues[x, y] -= dissipationRate;
            dirtyGasCells.Add(new Vector2Int(x, y));

            if (gasValues[x, y] <= 0 && !claimedCells[x, y])
            {
                gasValues[x, y] = 0;
                claimedCells[x, y] = true;
                dirtyClaimedCells.Add(new Vector2Int(x, y));
            }
        }
    }

    // Only touch GameObjects for cells that changed
    private void FlushVisuals()
    {
        foreach (var cell in dirtyGasCells)
        {
            int x = cell.x, y = cell.y;
            var ps = particles[x, y];
            if (ps == null) continue;

            var gas = gasValues[x, y];
            var emission = ps.emission;
            emission.rateOverTime = gas > 0.05f ? gas * 20f : 0f;

            // Only update start color if team is set
            var team = cellTeams[x, y];
            if (team != emptyTeam)
            {
                var main = ps.main;
                main.startColor = team.color;
            }
        }
        dirtyGasCells.Clear();

        foreach (var cell in dirtyClaimedCells)
        {
            int x = cell.x, y = cell.y;
            var mr = meshRenderers[x, y];
            if (mr == null) continue;

            var team = cellTeams[x, y];
            // Use a per-instance material block instead of mutating the shared material
            var block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", team.color); // or "_Color" for Built-in RP
            mr.SetPropertyBlock(block);
            mr.material = team.claimedMaterial;
            mr.enabled = true;
        }
        dirtyClaimedCells.Clear();
    }

    public void AddGas(Vector3 worldPos, float amount, Team t, int radius = 2)
    {
        Vector2Int center = WorldToGrid(worldPos);
        for (int x = -radius; x <= radius; x++)
        for (int y = -radius; y <= radius; y++)
        {
            int tx = center.x + x, ty = center.y + y;
            if (tx < 0 || tx >= width || ty < 0 || ty >= height) continue;

            gasValues[tx, ty] = Mathf.Clamp01(gasValues[tx, ty] + amount);
            dirtyGasCells.Add(new Vector2Int(tx, ty));

            if (cellTeams[tx, ty] != t)
            {
                if (teamCounts.ContainsKey(cellTeams[tx, ty]))
                    teamCounts[cellTeams[tx, ty]]--;
                cellTeams[tx, ty] = t;
                teamCounts.TryAdd(t, 0);
                teamCounts[t]++;
            }
        }
        UpdateUI();
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.z - originPosition.z) / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y) =>
        new(x * cellSize + originPosition.x, originPosition.y, y * cellSize + originPosition.z);

    private void UpdateUI()
    {
        if (teamCounts.Count > 0)
        {
            var cellTotal = width * height;
            var percentage = teamCounts.First().Value * 100 / cellTotal;
            uiManager.UpdateTerritory(percentage);
        }
        else
        {
            uiManager.UpdateTerritory(0);
        }
    }

    public float GetGas(int x, int y)  => gasValues[x, y];
    public Team  GetTeam(int x, int y) => cellTeams[x, y];
    public bool  IsClaimed(int x, int y) => claimedCells[x, y];
}