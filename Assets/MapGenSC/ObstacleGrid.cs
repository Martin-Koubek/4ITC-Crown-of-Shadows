using UnityEngine;

public class ObstacleGrid : MonoBehaviour
{
    public static ObstacleGrid Instance;

    public float tileSize = 4f;
    public int width = 200;
    public int height = 200;

    private bool[,] grid;

    private void Awake()
    {
        Instance = this;
        grid = new bool[width, height];
    }

    public void MarkRoom(Rect bounds)
    {
        for (int x = Mathf.FloorToInt(bounds.xMin / tileSize); x <= Mathf.CeilToInt(bounds.xMax / tileSize); x++)
        {
            for (int y = Mathf.FloorToInt(bounds.yMin / tileSize); y <= Mathf.CeilToInt(bounds.yMax / tileSize); y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                    grid[x, y] = true;
            }
        }
    }

    public bool IsBlocked(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return true;
        return grid[x, y];
    }
}