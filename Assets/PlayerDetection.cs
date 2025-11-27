using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player) && !player.PlayerBeenHit)
        {
            player.curentHealth -= enemy.dmg;
            player.PlayerBeenHit = true;
        }
        else return;
    }
}
