using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BossFireBall : MonoBehaviour
{
    public float dmg;
    public LayerMask floor;
    public LayerMask player;
    private GameObject _player;
    private GameObject _boss;

    public void Start()
    {
        _player = GameObject.Find("Knight");
        _boss = GameObject.Find("Skeleton_Rogue");
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == floor)
        {
            _boss.gameObject.TryGetComponent<Boss>(out Boss boss);
            if (Physics.CheckSphere(other.transform.position, 2, player))
            {
                _player.TryGetComponent<PlayerStats>(out PlayerStats stats);
                stats.curentHealth = -dmg;
                Destroy(boss.area);
                boss.canAttack = true;
            }
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == floor)
        {
            _boss.gameObject.TryGetComponent<Boss>(out Boss boss);
            if (Physics.CheckSphere(collision.transform.position, 2, player))
            {
                _player.TryGetComponent<PlayerStats>(out PlayerStats stats);
                stats.curentHealth = -dmg;
                Destroy(boss.area);
                boss.canAttack = true;
            }
            Destroy(gameObject);
        }
    }

}
