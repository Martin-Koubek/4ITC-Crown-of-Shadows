using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public startRoom startRoom;
    public EndRoom endRoom;

    private Room newRoom;
    private Room lastRoom;

    private Transform _connectionPointStart;
    private Transform _connectionPointEnd;

    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private GameObject _FloorTile;
    [SerializeField] private GameObject _Wall;
    System.Random rnd = new();

    private void Awake()
    {
    }
    public void NewFloor()
    {
        int roomCount = rnd.Next(5, 11);
        for (int i = 0; i < roomCount + 1; i++)
        {
            if(i == 0)
            {
                Vector3 roomPos = new Vector3(rnd.Next(50, 101), 0, rnd.Next(50, 101));
                _connectionPointStart = startRoom.connectionPoint;
                newRoom = Instantiate(rooms[rnd.Next(0, rooms.Count)], roomPos, Quaternion.identity);
                _connectionPointEnd = newRoom.connectionPoint;
                //Hall Gen script
                lastRoom = newRoom;

            }
            else if(i == roomCount + 1)
            {
                _connectionPointStart = lastRoom.connectionPoint;
                _connectionPointEnd = endRoom.connectionPoint;
                //Hall Gen script
            }
            else
            {
                Vector3 roomPos = new Vector3(rnd.Next(50, 101), 0, rnd.Next(50, 101));
                newRoom = Instantiate(rooms[rnd.Next(0, rooms.Count)], roomPos, Quaternion.identity);
                _connectionPointStart = lastRoom.connectionPoint;
                _connectionPointEnd = newRoom.connectionPoint;
                //Hall Gen script

                lastRoom = newRoom;
            }
        }
    }
}
