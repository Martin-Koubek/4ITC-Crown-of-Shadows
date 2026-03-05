using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public bool playerInRoom;

    public int basicHealth = 100;
    private int _maxHealth = 200;
    public float curentHealth = 200;

    [SerializeField]
    public bool canAttack = true;

    [SerializeField]
    private float baseDmg = 5;

    public float dmg = 20;
    public float shootForce = 50f;
    public float attackDelay = 10f;

    public Transform player;
    public bool hasBeenHit;

    public float timeBetweenAttacks;
    public bool attacked;

    public GameObject bUIReference;

    public GameObject areaMarker;
    public GameObject area;
    public BossFireBall FireBall;

    public Transform FirePoint;
    void Start()
    {
        player = GameObject.Find("Knight").transform;
        player.TryGetComponent<BossUI>(out BossUI bossUI);
        bUIReference = bossUI.UIReference;
    }

    void Update()
    {
        if (playerInRoom)
        {
            bUIReference.SetActive(true);
            Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
            transform.LookAt(playerPos);
            if (canAttack) 
            {
                AttackPlayer(); 
            }
            else return;
        }
        else return;

    }
    private IEnumerator waitingToAttack()
    {
        yield return new WaitForSeconds(30);
        yield return canAttack = true;
        StopCoroutine(waitingToAttack());
    }

    private void AttackPlayer()
    {
        canAttack = false;
        Vector3 areaSpot = new Vector3(player.position.x, 0, player.position.z);
        Vector3 attackSpot = new Vector3(player.position.x, player.position.y, player.position.z);
        area = Instantiate(areaMarker);
        area.transform.position = areaSpot;
        BossFireBall fireball;
        fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        fireball.TryGetComponent<Rigidbody>(out Rigidbody rig);
        fireball.TryGetComponent<BossFireBall>(out BossFireBall fire);
        fireball.transform.LookAt(attackSpot);

        fire.dmg = dmg;
        Vector3 direction = (attackSpot - FirePoint.position).normalized;

        rig.AddForce(direction * shootForce, ForceMode.Impulse);
    }
}
