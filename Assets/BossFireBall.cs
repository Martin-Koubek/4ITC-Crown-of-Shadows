using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BossFireBall : MonoBehaviour
{
    public float dmg;
    public LayerMask floor;
    public LayerMask player;
    private GameObject Player;

    public void Start()
    {
        Player = GameObject.Find("Knight");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == floor)
        {
            if(Physics.CheckSphere(collision.transform.position, 2, player)){
                Player.TryGetComponent<PlayerStats>(out PlayerStats stats);
                stats.curentHealth =- dmg;
            }
            Destroy(gameObject);
        }
    }
}
