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
        for (int i = 0; i < roomCount + 1; i++)
        {
            if (i == 0)
            {

                Vector3 roomPos = GetValidPos();

                _connectionPointStart = startRoom.connectionPoint;
                newRoom = Instantiate(rooms[rnd.Next(0, rooms.Count)], roomPos, Quaternion.identity);
                _connectionPointEnd = newRoom.connectionPoint;

                spawnedRooms.Add(newRoom);
                //Hall Gen script
                lastRoom = newRoom;


            }
            else if (i == roomCount + 1)
            {
                Vector3 roomPos = GetValidPos();
                newRoom = Instantiate(endRoom, roomPos, Quaternion.identity);
                _connectionPointStart = lastRoom.connectionPoint;
                _connectionPointEnd = endRoom.connectionPoint;

                spawnedRooms.Add(newRoom);
                //Hall Gen script
            }
            else
            {
                Vector3 roomPos = GetValidPos();
                newRoom = Instantiate(rooms[rnd.Next(0, rooms.Count)], roomPos, Quaternion.identity);
                _connectionPointStart = lastRoom.connectionPoint;
                _connectionPointEnd = newRoom.connectionPoint;

                //Hall Gen script

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
            pos = new Vector3(rnd.Next(50,101), 0, rnd.Next(50, 101));
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
}
