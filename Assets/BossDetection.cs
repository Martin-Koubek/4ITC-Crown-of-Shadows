using UnityEngine;

public class BossDetection : MonoBehaviour
{
    public int Damage;
    private Inventory inventory;
    private CombatController combatController;
    //public Transform player;
    private void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
        combatController = GetComponentInParent<CombatController>();
    }

    void Update()
    {
        if (inventory.CurentWeapon != null && inventory.CurentWeapon.gameObject.TryGetComponent<Sword>(out Sword sword))
        {
            Damage = sword.damage;
        }
        else if (inventory.CurentWeapon == null)
        {
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Boss>(out Boss boss) && combatController.isAttacking && !boss.hasBeenHit)
        {
            boss.curentHealth -= Damage;
            boss.hasBeenHit = true;
            boss.KnockBack();
        }
        else return;
    }
}
