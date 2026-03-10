using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float dmg;
    public float attackRange;

    public LayerMask wall;

    private IEnumerator DestroyFireBall(FireBall fireBall)
    {
        yield return new WaitForSeconds(attackRange + 5f);
        Destroy(gameObject);
    }
    private void Awake()
    {
        StartCoroutine(DestroyFireBall(this));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            player.curentHealth -= dmg;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & wall) != 0)
        {
            Destroy(gameObject);
        }
    }
}
