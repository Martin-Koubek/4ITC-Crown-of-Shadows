using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2 gridWorldSize = new(400, 400);
    public float nodeRadius = 3f;
    public float checkDistance;
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

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2f - Vector3.forward * gridWorldSize.y / 2f;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint =
                    worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !Physics.CheckSphere(worldPoint, checkDistance, unwalkableMask
                );

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }

        Debug.Log($"Grid created: {gridSizeX} x {gridSizeY}");
    }
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

        int[,] dirs = new int[,]
        {
        { 1, 0 },
        { -1, 0 },
        { 0, 1 },
        { 0, -1 }
        };

        for (int i = 0; i < 4; i++)
        {
            int checkX = node.gridX + dirs[i, 0];
            int checkY = node.gridY + dirs[i, 1];

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[checkX, checkY]);
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
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
    }
}
