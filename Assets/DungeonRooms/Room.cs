using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class Room : MonoBehaviour
{
    [Header("Enemy Spawns")]
    [SerializeField]
    private bool _Spawnable;

    [SerializeField]
    private List<Enemy> enemyL = new();

    [SerializeField]
    public Transform SpawnPoint;

    [Header("Connection points (doors)")]
    [SerializeField]
    public int maxCount;
    public List<ConnectionPoint> connectionPoints = new();

    Random rnd = new();
    private void Start()
    {
        // Automaticky najde všechny ConnectionPoint komponenty v childech
        if (connectionPoints.Count == 0)
        {
            connectionPoints.AddRange(
                GetComponentsInChildren<ConnectionPoint>()
            );
        }
        SpawnEnemy();
    }
    public void InicializujMistnost()
    {
        if (connectionPoints.Count == 0)
        {
            connectionPoints.AddRange(GetComponentsInChildren<ConnectionPoint>());
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
    private void SpawnEnemy()
    {
        int index = rnd.Next(0, enemyL.Count);
        int SpawnCount = rnd.Next(1, maxCount + 1);
        if (_Spawnable)
        {
            if (SpawnCount == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < SpawnCount; i++)
                {
                    Instantiate(enemyL[i], SpawnPoint);
                }
            }
        }
    }
}
