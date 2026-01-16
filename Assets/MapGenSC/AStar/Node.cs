using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public int fCost => gCost + hCost;

    public Node(bool walkable, Vector3 worldPos, int x, int y)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
        gridX = x;
        gridY = y;
    }
}
