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
    private Enemy spawn;
    Random rnd = new();

    void Update()
    {
        if (isDead)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        spawn = Instantiate(enemies[rnd.Next(0, enemies.Count)], spawnSpot);
        spawn.gameObject.transform.position = spawnSpot.transform.position;
        isDead = false;
    }
}
