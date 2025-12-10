using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    public static List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        float tile = ObstacleGrid.Instance.tileSize;

        Vector2Int startNode = new Vector2Int(Mathf.RoundToInt(start.x / tile), Mathf.RoundToInt(start.z / tile));
        Vector2Int endNode = new Vector2Int(Mathf.RoundToInt(end.x / tile), Mathf.RoundToInt(end.z / tile));

        var open = new PriorityQueue<PathNode>();
        var all = new Dictionary<Vector2Int, PathNode>();

        PathNode startN = new PathNode(startNode, null, 0, Heuristic(startNode, endNode));
        open.Enqueue(startN);
        all[startNode] = startN;

        while (open.Count > 0)
        {
            PathNode current = open.Dequeue();

            if (current.pos == endNode)
                return Reconstruct(current, tile);

            foreach (var dir in directions)
            {
                Vector2Int next = current.pos + dir;

                if (ObstacleGrid.Instance.IsBlocked(next.x, next.y)) continue;

                float newG = current.g + 1;
                if (!all.ContainsKey(next) || newG < all[next].g)
                {
                    PathNode node = new PathNode(next, current, newG, Heuristic(next, endNode));
                    all[next] = node;
                    open.Enqueue(node);
                }
            }
        }

        return null;
    }

    static Vector2Int[] directions =
    {
        new Vector2Int(1,0), new Vector2Int(-1,0),
        new Vector2Int(0,1), new Vector2Int(0,-1)
    };

    static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    static List<Vector3> Reconstruct(PathNode node, float tile)
    {
        List<Vector3> points = new List<Vector3>();

        while (node != null)
        {
            points.Add(new Vector3(node.pos.x * tile, 0, node.pos.y * tile));
            node = node.parent;
        }

        points.Reverse();
        return points;
    }

    public class PathNode : System.IComparable<PathNode>
    {
        public Vector2Int pos;
        public PathNode parent;
        public float g;
        public float h;
        public float F => g + h;

        public PathNode(Vector2Int pos, PathNode parent, float g, float h)
        {
            this.pos = pos;
            this.parent = parent;
            this.g = g;
            this.h = h;
        }

        public int CompareTo(PathNode other) => F.CompareTo(other.F);
    }
}