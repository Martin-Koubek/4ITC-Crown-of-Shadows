using UnityEngine;
using System.Collections;

public class EnemyDetection : MonoBehaviour
{
    public int Damage;
    private Inventory inventory;
    private CombatController combatController;
    public Transform player;


    private void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
        combatController = GetComponentInParent<CombatController>();
    }
    private void Update()
    {
        if (inventory.CurentWeapon != null && inventory.CurentWeapon.gameObject.TryGetComponent<Sword>(out Sword sword))
        {
            Damage = sword.damage;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && combatController.isAttacking && !enemy.hasBeenHit)
        {
            enemy.curentHealth -= Damage;
            enemy.hasBeenHit = true;
            enemy.KnockBack();
        }
        else return;
    }
}
