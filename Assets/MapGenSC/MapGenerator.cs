using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Room startRoom;
    public Room endRoom;

    private Room lastRoom;

    public float minDistance = 50f;

    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private GameObject _FloorTile;

    [SerializeField] private List<Room> spawnedRooms = new List<Room>();

    System.Random rnd = new();

    [SerializeField] private Pathfinding pathfinder;

    private void Start()
    {

        NewFloor();
        CreateCorridors();

    }

    public void NewFloor()
    {
        int roomCount = rnd.Next(5, 11);
        for (int i = 0; i <= roomCount; i++)
        {
            Vector3 roomPos = GetValidPos();
            Room newRoom;

            // EndRoom
            if (i == roomCount)
            {
                newRoom = Instantiate(endRoom, roomPos, Quaternion.identity);
            }
            else
            {
                newRoom = Instantiate(rooms[rnd.Next(rooms.Count)], roomPos, Quaternion.identity);
            }

            spawnedRooms.Add(newRoom);

            lastRoom = newRoom;
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
    void CreateCorridors()
    {
        for (int i = 0; i < spawnedRooms.Count - 1; i++)
        {
            Room fromRoom = spawnedRooms[i];
            Room toRoom = spawnedRooms[i + 1];

            ConnectionPoint fromDoor = fromRoom.GetFreeConnectionPoint();
            ConnectionPoint toDoor = toRoom.GetFreeConnectionPoint();

            if (fromDoor == null || toDoor == null)
            {
                Debug.LogWarning("Room has no free connections");
                continue;
            }

            fromDoor.used = true;
            toDoor.used = true;

            Vector3 start = fromDoor.transform.position;
            Vector3 end = toDoor.transform.position;

            var path = pathfinder.FindPath(start, end);

            foreach (var node in path)
            {
                Instantiate(_FloorTile, node.worldPos, Quaternion.identity);
            }
        }
    }

}