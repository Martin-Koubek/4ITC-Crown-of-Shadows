using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player) && !player.hasBeenHit)
        {
            player.curentHealth -= enemy.dmg;
            player.hasBeenHit = true;
        }
        else return;
    }
}
