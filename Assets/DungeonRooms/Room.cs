using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Connection points (doors)")]
    public List<ConnectionPoint> connectionPoints = new();

    private void Awake()
    {
        // Automaticky najde všechny ConnectionPoint komponenty v childech
        if (connectionPoints.Count == 0)
        {
            connectionPoints.AddRange(
                GetComponentsInChildren<ConnectionPoint>()
            );
        }
    }

    public ConnectionPoint GetFreeConnectionPoint()
    {
        foreach (var point in connectionPoints)
        {
            if (!point.used)
            {
                point.used = true;
                return point;
            }
        }

        return null; // žádné volné dveøe
    }
}
