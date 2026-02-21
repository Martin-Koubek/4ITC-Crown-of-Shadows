using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform PlayerRef;

    public Room startRoom;
    public Room endRoom;

    [SerializeField]
    private NavMeshSurface _navMeshSurface;

    public int _currentLevel = 1;

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
        pathFinder.grid.RebuildGrid();
        CreateCorridors();
        _navMeshSurface.BuildNavMesh();
    }

    public void NewFloor()
    {
        int roomCount = rnd.Next(5, 6 + _currentLevel);
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

        Vector3 GetValidPos()
        {
            Vector3 pos;
            do
            {
                pos = new Vector3(rnd.Next(-100, 111), 0, rnd.Next(50, 150));
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
        ConnectionPoint fromDoor = fromRoom.GetFreeConnectionPoint();
        ConnectionPoint toDoor = toRoom.GetFreeConnectionPoint();

        if (fromDoor == null || toDoor == null) return;

        fromDoor.used = true;
        toDoor.used = true;

        Vector3 doorPosStart = fromDoor.transform.position;
        Vector3 corridorStart = fromDoor.GetCorridorStart();

        Vector3 doorPosEnd = toDoor.transform.position;
        Vector3 corridorEnd = toDoor.GetCorridorStart();

        FillGapWithTiles(doorPosStart, corridorStart);
        FillGapWithTiles(doorPosEnd, corridorEnd);

        Node startNode = pathFinder.grid.NodeFromWorldPoint(corridorStart);
        Node targetNode = pathFinder.grid.NodeFromWorldPoint(corridorEnd);
        if (startNode != null) startNode.walkable = true;
        if (targetNode != null) targetNode.walkable = true;

        var path = pathFinder.FindPath(corridorStart, corridorEnd);

        if (path != null)
        {
            foreach (var node in path)
            {
                GameObject newTile = Instantiate(_FloorTile, node.worldPos, Quaternion.identity, mapGen);
                spawnedFloors.Add(newTile);
            }
        }
    }
    private void FillGapWithTiles(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        int steps = Mathf.CeilToInt(distance / 2f);

        for (int i = 0; i <= steps; i++)
        {
            Vector3 pos = Vector3.Lerp(start, end, (float)i / steps);
            pos.y = 0;
            GameObject newTile = Instantiate(_FloorTile, pos, Quaternion.identity, mapGen);
            spawnedFloors.Add(newTile);
        }
    }
    public void NewFloorGen()
    {
        NewFloor();
        Instantiate(startRoom, startRoomPos, Quaternion.identity);
        pathFinder.grid.RebuildGrid();
        CreateCorridors();
        _navMeshSurface.BuildNavMesh();

    }

    public IEnumerator NewFloorGenerator()
    {
        //blindfold
        NewFloor();
        pathFinder.grid.RebuildGrid();
        CreateCorridors();
        _navMeshSurface.BuildNavMesh();
        yield return new WaitForEndOfFrame();
        //unblind
    }

    public void resetPlayer()
    {
        PlayerRef.transform.position = Vector3.zero;
    }

}
