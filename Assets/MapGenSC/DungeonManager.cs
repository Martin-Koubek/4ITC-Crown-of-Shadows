using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public List<Room> vygenerovaneMistnosti = new List<Room>();
    public CorridorGenerator corridorGen; // Pøetáhnìte objekt s CorridorGenerator v Inspectoru

    private System.Random rnd = new();

    public Room startRoom;
    public Room endRoom;
    [SerializeField] public List<Room> rooms = new List<Room>();

    public float minDistance = 50f;

    void Start()
    {
        GenerujDungeon();
    }

    void GenerujDungeon()
    {
        int roomCount = rnd.Next(5, 6);

        for (int i = 0; i <= roomCount; i++)
        {
            Vector3 roomPos = GetValidPos();
            Room newRoom;

            if (i == roomCount)
                newRoom = Instantiate(endRoom, roomPos, Quaternion.identity);
            else
                newRoom = Instantiate(rooms[rnd.Next(rooms.Count)], roomPos, Quaternion.identity);
            vygenerovaneMistnosti.Add(newRoom);
        }

        Vector3 GetValidPos()
        {
            Vector3 pos;
            do
            {
                pos = new Vector3(rnd.Next(-100, 151), 0, rnd.Next(50, 200));
            }
            while (isTooClose(pos, vygenerovaneMistnosti, minDistance));

            return pos;
        }


        corridorGen.PropojMistnosti(vygenerovaneMistnosti);
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