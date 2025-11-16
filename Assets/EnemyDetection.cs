using UnityEngine;
using System.Collections;

public class EnemyDetection : MonoBehaviour
{
    public int Damage;
    public Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        Damage = 10;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && collision.gameObject.GetComponent<Hittabel>())
        {
            enemy.Health =- Damage;
        }
    }
}
