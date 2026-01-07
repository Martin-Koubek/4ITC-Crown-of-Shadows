using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public Room startRoom;
    public Room endRoom;

    [SerializeField]
    private NavMeshSurface _navMeshSurface;

    [SerializeField]
    private Transform mapGen;

    public float minDistance = 50f;

    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private GameObject _FloorTile;

    [SerializeField] private List<Room> spawnedRooms = new List<Room>();

    [SerializeField] private Pathfinding pathfinder;

    private System.Random rnd = new();

    [SerializeField] private float floorOffset = 2f;


    private void Start()
    {
        NewFloor();
        pathfinder.grid.RebuildGrid();
        CreateCorridors();
        _navMeshSurface.BuildNavMesh();
    }

    public void NewFloor()
    {
        int roomCount = rnd.Next(5, 11);

        for (int i = 0; i <= roomCount; i++)
        {
            Vector3 roomPos = GetValidPos();
            Room newRoom;

            if (i == roomCount)
                newRoom = Instantiate(endRoom,roomPos, Quaternion.identity);
            else
                newRoom = Instantiate(rooms[rnd.Next(rooms.Count)], roomPos, Quaternion.identity);

            spawnedRooms.Add(newRoom);
        }

        Vector3 GetValidPos()
        {
            Vector3 pos;
            do
            {
                pos = new Vector3(rnd.Next(-200, 201), 0, rnd.Next(50, 201));
            }
            while (isTooClose(pos, spawnedRooms, minDistance));

            return pos;
        }

        bool isTooClose(Vector3 pos, List<Room> spawnedRooms, float minDistance)
        {
            foreach (var r in spawnedRooms)
            {
                if (Vector3.Distance(pos, r.transform.position) <= minDistance)
                    return true;
            }
            return false;
        }
    }

    private void CreateCorridors()
    {
        // Smyčka přes všechny sousední místnosti
        for (int i = 0; i < spawnedRooms.Count - 1; i++)
        {
            Room fromRoom = spawnedRooms[i];
            Room toRoom = spawnedRooms[i + 1];

            CreateCorridorBetween(fromRoom, toRoom);
        }

    }

    // Funkce, která vytvoří chodbu mezi dvěma místnostmi
    private void CreateCorridorBetween(Room fromRoom, Room toRoom)
    {
        ConnectionPoint fromDoor = fromRoom.GetFreeConnectionPoint();
        ConnectionPoint toDoor = toRoom.GetFreeConnectionPoint();

        if (fromDoor == null || toDoor == null)
        {
            Debug.LogWarning("Room has no free connections");
            return;
        }

        fromDoor.used = true;
        toDoor.used = true;

        // Automatický posun start/end ven z místnosti podle směru dveří
        Vector3 start = fromDoor.GetCorridorStart();
        Vector3 end = toDoor.GetCorridorStart();

        Node startNode = pathfinder.grid.NodeFromWorldPoint(start);
        Node targetNode = pathfinder.grid.NodeFromWorldPoint(end);

        Debug.Log($"FROM door: {fromDoor.transform.position}, startNode walkable: {startNode.walkable}");
        Debug.Log($"TO door: {toDoor.transform.position}, targetNode walkable: {targetNode.walkable}");

        if (!startNode.walkable || !targetNode.walkable)
        {
            Debug.LogWarning("Skipping corridor: start or target not walkable");
            return;
        }

        var path = pathfinder.FindPath(start, end);

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("Pathfinding returned null or empty path");
            return;
        }

        foreach (var node in path)
        {
            Instantiate(_FloorTile, node.worldPos, Quaternion.identity);
        }
    }
}
