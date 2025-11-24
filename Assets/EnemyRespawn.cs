using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class EnemyRespawn : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    private int respawnTime = 5;
    public bool isDead = false;
    public Transform spawnSpot;
    private bool spawned = false;
    Random rnd = new();

    void Update()
    {
        if (isDead)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Instantiate(enemies[1], spawnSpot);
        isDead = false;
    }
}
