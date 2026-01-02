using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2 gridWorldSize = new(400, 400);
    public float nodeRadius = 1f;
    public LayerMask unwalkableMask;

    public Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.FloorToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.FloorToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid(); // ❗ DŮLEŽITÉ
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft =
            transform.position
            - Vector3.right * gridWorldSize.x / 2f
            - Vector3.forward * gridWorldSize.y / 2f;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint =
                    worldBottomLeft
                    + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !Physics.CheckSphere(
                    worldPoint,
                    nodeRadius,
                    unwalkableMask
                );

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }

        Debug.Log($"Grid created: {gridSizeX} x {gridSizeY}");
    }

    // ✅ TADY JE TO, CO JSI CHTĚL
    public IEnumerable<Node> AllNodes
    {
        get
        {
            foreach (var n in grid)
                yield return n;
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        Vector3 localPos = worldPos - transform.position;

        float halfX = gridWorldSize.x / 2f;
        float halfY = gridWorldSize.y / 2f;

        if (localPos.x < -halfX || localPos.x > halfX ||
            localPos.z < -halfY || localPos.z > halfY)
        {
            Debug.LogError($"OUTSIDE GRID! WorldPos: {worldPos}, LocalPos: {localPos}");
            return null;
        }

        float percentX = (localPos.x + halfX) / gridWorldSize.x;
        float percentY = (localPos.z + halfY) / gridWorldSize.y;

        int x = Mathf.Clamp(Mathf.FloorToInt(percentX * gridSizeX), 0, gridSizeX - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt(percentY * gridSizeY), 0, gridSizeY - 1);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX &&
                    checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public void RebuildGrid()
    {
        CreateGrid();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(gridWorldSize.x, 1, gridWorldSize.y)
        );
    }
}
