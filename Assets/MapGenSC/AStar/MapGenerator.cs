using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform PlayerRef;

    public Room startRoom;
    public Room BossWeaponRoom;
    public Room endRoom;
    public GameObject BossRoom;


    [SerializeField]
    private NavMeshSurface _navMeshSurface;

    public int currentLevel = 1;

    [SerializeField]
    private Transform mapGen;

    public float minDistance = 50f;

    [SerializeField] public List<Room> rooms = new List<Room>();
    [SerializeField] private GameObject _FloorTile;
    [SerializeField] public List<Room> spawnedRooms = new List<Room>();
    public List<GameObject> spawnedFloors = new List<GameObject>();

    [SerializeField] private PathFinding pathFinder;

    private System.Random rnd = new();

    [SerializeField] private float floorOffset = 2f;

    private Vector3 startRoomPos = Vector3.zero;
    private void Start()
    {
        NewFloor();
        Physics.SyncTransforms();
        pathFinder.grid.RebuildGrid();
        CreateCorridors();
        _navMeshSurface.BuildNavMesh();
    }
    public void NewFloor()
    {
        if (currentLevel == 1)
        {
            int roomCount = rnd.Next(5, 6);
            for (int i = 0; i <= roomCount; i++)
            {
                Vector3 roomPos = GetValidPos();
                Room newRoom;

                if (i == roomCount)
                    newRoom = Instantiate(endRoom, roomPos, Quaternion.identity, mapGen);
                else if (i == roomCount - 1)
                {
                    newRoom = Instantiate(BossWeaponRoom, roomPos, Quaternion.identity, mapGen);
                }
                else
                    newRoom = Instantiate(rooms[rnd.Next(rooms.Count)], roomPos, Quaternion.identity, mapGen);

                spawnedRooms.Add(newRoom);
            }
        }
        else if (currentLevel == 3)
        {
            Instantiate(BossRoom, Vector3.zero, Quaternion.identity, mapGen);
            Instantiate(startRoom, Vector3.zero, Quaternion.identity, mapGen);
        }
        else
        {
            int roomCount = rnd.Next(5, 6);
            for (int i = 0; i <= roomCount; i++)
            {
                Vector3 roomPos = GetValidPos();
                Room newRoom;

                if (i == roomCount)
                    newRoom = Instantiate(endRoom, roomPos, Quaternion.identity, mapGen);
                else
                    newRoom = Instantiate(rooms[rnd.Next(rooms.Count)], roomPos, Quaternion.identity, mapGen);

                spawnedRooms.Add(newRoom);
            }
        }

        Vector3 GetValidPos()
        {
            Vector3 pos;
            int safetyNet = 0; // Pojistka
            do
            {
                // Snap points to exactly multiples of 6 (assuming grid nodeRadius = 3 -> diameter = 6)
                // This ensures all rooms spawn perfectly aligned with the A* nodes
                int rx = rnd.Next(-17, 19) * 6;
                int rz = rnd.Next(8, 25) * 6;
                
                pos = new Vector3(rx, 0, rz);
                safetyNet++;
                if (safetyNet > 100) break; // Pokud nenajde místo po 100 pokusech, prostě to zkusí tady
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
    private void CreateCorridorBetween(Room fromRoom, Room toRoom)
    {
        ConnectionPoint bestFromDoor = null;
        ConnectionPoint bestToDoor = null;
        float shortestDistance = float.MaxValue;

        // Ensure connections points are loaded if they weren't somehow
        if (fromRoom.connectionPoints.Count == 0) fromRoom.InicializujMistnost();
        if (toRoom.connectionPoints.Count == 0) toRoom.InicializujMistnost();

        // Find the absolute closest pair of doors between these two rooms
        foreach (var fDoor in fromRoom.connectionPoints)
        {
            if (fDoor.used) continue;
            
            foreach (var tDoor in toRoom.connectionPoints)
            {
                if (tDoor.used) continue;

                float dist = Vector3.Distance(fDoor.transform.position, tDoor.transform.position);
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    bestFromDoor = fDoor;
                    bestToDoor = tDoor;
                }
            }
        }

        if (bestFromDoor == null || bestToDoor == null) 
        {
            Debug.LogWarning($"No free doors between {fromRoom.name} and {toRoom.name}");
            return;
        }

        bestFromDoor.used = true;
        bestToDoor.used = true;

        ConnectionPoint fromDoor = bestFromDoor;
        ConnectionPoint toDoor = bestToDoor;

        Vector3 doorPosStart = fromDoor.transform.position;
        Vector3 corridorStart = doorPosStart; // The connection point is already perfectly spaced

        Vector3 doorPosEnd = toDoor.transform.position;
        Vector3 corridorEnd = doorPosEnd;
        
        // As you suggested, we just instantiate the tile EXACTLY at the connection point!
        GameObject forceTile1 = Instantiate(_FloorTile, doorPosStart, Quaternion.identity, mapGen);
        spawnedFloors.Add(forceTile1);
        
        GameObject forceTile2 = Instantiate(_FloorTile, doorPosEnd, Quaternion.identity, mapGen);
        spawnedFloors.Add(forceTile2);

        Node startNode = pathFinder.grid.NodeFromWorldPoint(corridorStart);
        Node targetNode = pathFinder.grid.NodeFromWorldPoint(corridorEnd);
        if (startNode != null) startNode.walkable = true;
        if (targetNode != null) targetNode.walkable = true;

        var path = pathFinder.FindPath(corridorStart, corridorEnd);
        if (path == null) Debug.LogWarning($"Cesta nenalezena mezi {fromRoom.name} a {toRoom.name}");

        if (path != null)
        {
            foreach (var node in path)
            {
                if (!node.isPaved)
                {
                    GameObject newTile = Instantiate(_FloorTile, node.worldPos, Quaternion.identity, mapGen);
                    spawnedFloors.Add(newTile);
                    node.isPaved = true;
                }
            }
        }
    }

    public void NewFloorGen()
    {
        if (currentLevel == 3)
        {
            spawnedRooms.Clear();
            spawnedFloors.Clear();

            NewFloor();
            Physics.SyncTransforms();
            pathFinder.grid.RebuildGrid();
            _navMeshSurface.BuildNavMesh();
        }
        else
        {
            spawnedRooms.Clear();
            spawnedFloors.Clear();


            Room newStartRoom = Instantiate(startRoom, startRoomPos, Quaternion.identity, mapGen);
            newStartRoom.InicializujMistnost();
            spawnedRooms.Insert(0, newStartRoom);


            NewFloor();

            Physics.SyncTransforms();
            pathFinder.grid.RebuildGrid();
            CreateCorridors();
            _navMeshSurface.BuildNavMesh();

        }
    }
}

