using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public startRoom startRoom;
    public Room endRoom;

    private Room newRoom;
    private Room lastRoom;

    public float minDistance = 50f;

    private Transform _connectionPointStart;
    private Transform _connectionPointEnd;

    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private GameObject _FloorTile;
    [SerializeField] private GameObject _Wall;

    [SerializeField] private List<Room> spawnedRooms = new List<Room>();

    System.Random rnd = new();

    private void Awake()
    {
        NewFloor();
    }
    public void NewFloor()
    {
        int roomCount = rnd.Next(5, 11);
        for (int i = 0; i < roomCount; i++)
        {
            if (i == 0)
            {

                Vector3 roomPos = GetValidPos();

                _connectionPointStart = startRoom.connectionPoint;
                newRoom = Instantiate(rooms[rnd.Next(0, rooms.Count)], roomPos, Quaternion.identity);
                _connectionPointEnd = newRoom.connectionPoint;

                spawnedRooms.Add(newRoom);
                CreateHallway(_connectionPointStart.position, _connectionPointEnd.position);
                lastRoom = newRoom;


            }
            else if (i == roomCount)
            {
                Vector3 roomPos = GetValidPos();
                newRoom = Instantiate(endRoom, roomPos, Quaternion.identity);
                _connectionPointStart = lastRoom.connectionPoint;
                _connectionPointEnd = endRoom.connectionPoint;

                spawnedRooms.Add(newRoom);
                CreateHallway(_connectionPointStart.position, _connectionPointEnd.position);
            }
            else
            {
                Vector3 roomPos = GetValidPos();
                newRoom = Instantiate(rooms[rnd.Next(0, rooms.Count)], roomPos, Quaternion.identity);
                _connectionPointStart = lastRoom.connectionPoint;
                _connectionPointEnd = newRoom.connectionPoint;

                CreateHallway(_connectionPointStart.position, _connectionPointEnd.position);

                spawnedRooms.Add(newRoom);
                lastRoom = newRoom;


            }
        }
    }

    Vector3 GetValidPos()
    {
        Vector3 pos;
        do
        {
            pos = new Vector3(rnd.Next(-200,201), 0, rnd.Next(50, 201));
        } while (isTooClose(pos, spawnedRooms, minDistance));

        return pos;
    }

    bool isTooClose(Vector3 pos, List<Room> spawnedRooms, float minDistance)
    {
        foreach (var r in spawnedRooms)
        {
            if (Vector3.Distance(pos, r.transform.position) <= minDistance)
            {
                return true;
            }
        }
        return false;
    }
    void CreateHallway(Vector3 start, Vector3 end)
    {
        // Uděláme L-tvar: nejdřív X osa → pak Z osa
        Vector3 corner = new Vector3(end.x, start.y, start.z);

        CreateSegment(start, corner);
        CreateSegment(corner, end);
    }

    void CreateSegment(Vector3 a, Vector3 b)
    {
        Vector3 dir = (b - a).normalized;
        float distance = Vector3.Distance(a, b);
        int steps = Mathf.RoundToInt(distance);

        for (int i = 0; i < steps; i++)
        {
            Vector3 pos = a + dir * i;

            // Podlaha chodby
            Instantiate(_FloorTile, pos, Quaternion.identity);

          
        }
    }
}
