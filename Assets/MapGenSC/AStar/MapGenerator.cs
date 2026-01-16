using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

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
        int roomCount = rnd.Next(5, 6 + _currentLevel);

        for (int i = 0; i <= roomCount; i++)
        {
            Vector3 roomPos = GetValidPos();
            Room newRoom;

            if (i == roomCount)
                newRoom = Instantiate(endRoom,roomPos, Quaternion.identity, mapGen);
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

    // Funkce, která vytvoří chodbu mezi dvěma místnostmi
    private void CreateCorridorBetween(Room fromRoom, Room toRoom)
    {
        ConnectionPoint fromDoor = fromRoom.GetFreeConnectionPoint();
        ConnectionPoint toDoor = toRoom.GetFreeConnectionPoint();

        if (fromDoor == null || toDoor == null) return;

        fromDoor.used = true;
        toDoor.used = true;

        // 1. Získání bodů
        Vector3 doorPosStart = fromDoor.transform.position;
        Vector3 corridorStart = fromDoor.GetCorridorStart(); // Bod vysunutý ven z místnosti

        Vector3 doorPosEnd = toDoor.transform.position;
        Vector3 corridorEnd = toDoor.GetCorridorStart();

        // 2. NAPE VNO PŘIDAT DLAŽDICE PŘED DVEŘE (vytvoření spojovacího krčku)
        // Tento cyklus položí dlaždice od dveří až k bodu, kde začíná pathfinding
        FillGapWithTiles(doorPosStart, corridorStart);
        FillGapWithTiles(doorPosEnd, corridorEnd);

        // 3. Vynucení průchodnosti v gridu (aby pathfinding tyto body nezahodil)
        Node startNode = pathfinder.grid.NodeFromWorldPoint(corridorStart);
        Node targetNode = pathfinder.grid.NodeFromWorldPoint(corridorEnd);
        if (startNode != null) startNode.walkable = true;
        if (targetNode != null) targetNode.walkable = true;

        // 4. Samotný pathfinding
        var path = pathfinder.FindPath(corridorStart, corridorEnd);

        if (path != null)
        {
            foreach (var node in path)
            {
                Instantiate(_FloorTile, node.worldPos, Quaternion.identity, mapGen);
            }
        }
    }

    // Pomocná metoda pro vyplnění mezery mezi dveřmi a gridem
    private void FillGapWithTiles(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        // Počet dlaždic závisí na vzdálenosti, dáváme dlaždici každé 2 jednotky (nebo dle velikosti tile)
        int steps = Mathf.CeilToInt(distance / 2f);

        for (int i = 0; i <= steps; i++)
        {
            Vector3 pos = Vector3.Lerp(start, end, (float)i / steps);
            // Zarovnání na Y (případně přidat floorOffset)
            pos.y = 0;
            Instantiate(_FloorTile, pos, Quaternion.identity, mapGen);
        }
    }
    public void NewFloorGen()
    {
        NewFloor();
        pathfinder.grid.RebuildGrid();
        CreateCorridors();
        _navMeshSurface.BuildNavMesh();
    }

    public IEnumerator NewFloorGenerator()
    {
        //blindfold
        NewFloor();
        pathfinder.grid.RebuildGrid();
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
