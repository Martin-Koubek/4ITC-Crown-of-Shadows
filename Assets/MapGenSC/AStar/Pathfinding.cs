using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public GridGenerator grid;

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode == null || targetNode == null)
        {
            Debug.LogError("Start or target node is null!");
            return null;
        }

        if (!startNode.walkable || !targetNode.walkable)
        {
            Debug.LogError($"Start or target NOT walkable | start: {startNode.walkable}, target: {targetNode.walkable}");
            return null;
        }

        // ? DÙLEŽITÉ – reset node dat
        foreach (Node node in grid.AllNodes)
        {
            node.gCost = int.MaxValue;
            node.hCost = 0;
            node.parent = null;
        }

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        List<Node> openSet = new();
        HashSet<Node> closedSet = new();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            // ? lepší výbìr node (fCost + hCost)
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost &&
                    openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newCost < neighbour.gCost)
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        Debug.LogWarning("Path NOT found");
        return null;
    }

    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;

            if (current == null)
            {
                Debug.LogError("Broken path (parent is null)");
                return null;
            }
        }

        path.Reverse();
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);
        return 10 * (dstX + dstY);
    }
}
