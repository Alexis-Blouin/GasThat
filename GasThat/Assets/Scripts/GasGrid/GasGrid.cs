using System.Collections.Generic;
using UnityEngine;

public class GasGrid : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    public float cellSize = 2f;
    public Vector3 originPosition;

    public float spreadRate = 0.05f;
    public float dissipationRate = 0.02f;
    private float spreadTimer;
    public float spreadInterval = 0.5f;

    private float[,] gasValues;
    private Color[,] cellColors;

    private Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();
    
    public GameObject gasCellPrefab;

    void Start()
    {
        gasValues = new float[width, height];
        cellColors = new Color[width, height];
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 pos = GridToWorld(x, y);
            GameObject cell = Instantiate(gasCellPrefab, pos, Quaternion.identity);
            var gasCell = cell.GetComponent<GasCell>();
            gasCell.gridX = x;
            gasCell.gridY = y;
        }
    }
    
    void Update()
    {
        spreadTimer += Time.deltaTime;
        if (spreadTimer >= spreadInterval)
        {
            spreadTimer = 0f;
            //Spread();
            Dissipate();
        }
    }

    // void Spread()
    // {
    //     float[,] newValues = (float[,])gasValues.Clone();
    //
    //     for (int x = 0; x < width; x++)
    //     for (int y = 0; y < height; y++)
    //     {
    //         if (gasValues[x, y] <= 0) continue;
    //
    //         // Spread to neighbors
    //         int[] dx = { 1, -1, 0, 0 };
    //         int[] dy = { 0, 0, 1, -1 };
    //         for (int i = 0; i < 4; i++)
    //         {
    //             int nx = x + dx[i], ny = y + dy[i];
    //             if (nx >= 0 && nx < width && ny >= 0 && ny < height)
    //                 newValues[nx, ny] = Mathf.Clamp01(newValues[nx, ny] + gasValues[x, y] * spreadRate);
    //         }
    //     }
    //
    //     gasValues = newValues;
    // }
    
    private void Dissipate()
    {
        float[,] newValues = (float[,])gasValues.Clone();

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            if (gasValues[x, y] <= 0) continue;

            // Dissipate
            newValues[x, y] -= dissipationRate;
        }

        gasValues = newValues;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.z - originPosition.z) / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(x * cellSize + originPosition.x, 0, y * cellSize + originPosition.z);
    }

    public void AddGas(Vector3 worldPos, float amount, Color color, int radius = 2)
    {
        Vector2Int cell = WorldToGrid(worldPos);
        for (int x = -radius; x <= radius; x++)
        for (int y = -radius; y <= radius; y++)
        {
            int tx = cell.x + x, ty = cell.y + y;
            if (tx >= 0 && tx < width && ty >= 0 && ty < height)
            {
                gasValues[tx, ty] = Mathf.Clamp01(gasValues[tx, ty] + amount);
                if (cellColors[tx, ty] != color)
                {
                    if(colorCounts.ContainsKey(cellColors[tx, ty]))
                        colorCounts[cellColors[tx, ty]]--;
                    cellColors[tx, ty] = color;
                    if(colorCounts.ContainsKey(color))
                        colorCounts[color]++;
                    else
                        colorCounts.Add(color, 1);
                }
            }
        }
    }

    public void PrintTerritory()
    {
        var cellTotal = width * height;
        foreach (var entry in colorCounts)
        {
            Debug.Log(entry.Key + ": " + entry.Value + " / " + cellTotal);
        }
    }

    public float GetGas(int x, int y) => gasValues[x, y];
    public Color GetCellColor(int x, int y) => cellColors[x, y];
    public float GetGas(Vector3 worldPos)
    {
        var c = WorldToGrid(worldPos);
        return gasValues[c.x, c.y];
    }
    public Color GetColor(Vector3 worldPos)
    {
        var c = WorldToGrid(worldPos);
        return cellColors[c.x, c.y];
    }
}
