using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public startRoom startRoom;
    public Room endRoom;

    private Room lastRoom;

    public float minDistance = 50f;

    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private GameObject _FloorTile;

    [SerializeField] private List<Room> spawnedRooms = new List<Room>();

    System.Random rnd = new();

    private void Awake()
    {
        NewFloor();
    }

    public void NewFloor()
    {
        int roomCount = rnd.Next(5, 11);
        MarkStartRoomAsObstacle();
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
            MarkRoomAsObstacle(newRoom);

            // --- FIRST ROOM → connect to startRoom ---
            if (i == 0)
            {
                CreateHallway(startRoom.connectionPoint.position, newRoom.connectionPoint.position);
            }
            else
            {
                // Other rooms connect to previous room
                CreateHallway(spawnedRooms[i - 1].connectionPoint.position,
                              newRoom.connectionPoint.position);
            }

            lastRoom = newRoom;
        }


        // ---------- HELPERS BELOW ----------

        void MarkRoomAsObstacle(Room room)
        {
            Bounds b = room.GetComponent<Collider>().bounds;
            Rect r = new Rect(b.min.x, b.min.z, b.size.x, b.size.z);
            ObstacleGrid.Instance.MarkRoom(r);
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

        void CreateHallway(Vector3 start, Vector3 end)
        {
            List<Vector3> path = AStarPathfinder.FindPath(start, end);
            if (path == null) return;

            float spacing = 6f;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 a = path[i];
                Vector3 b = path[i + 1];

                float dist = Vector3.Distance(a, b);
                int steps = Mathf.CeilToInt(dist / spacing);

                for (int s = 0; s < steps; s++)
                {
                    Vector3 pos = Vector3.Lerp(a, b, s / (float)steps);
                    Instantiate(_FloorTile, pos, Quaternion.identity);
                }
            }
        }
        void MarkStartRoomAsObstacle()
        {
            Bounds b = startRoom.GetComponent<Collider>().bounds;
            Rect r = new Rect(b.min.x, b.min.z, b.size.x, b.size.z);
            ObstacleGrid.Instance.MarkRoom(r);
        }
    }
}