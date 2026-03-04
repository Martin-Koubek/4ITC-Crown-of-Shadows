using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public bool playerInRoom;

    public int basicHealth = 100;
    private int _maxHealth = 100;
    public float curentHealth = 100;

    private bool canAttack = true;
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
    public BossFireBall FireBall;

    public Transform FirePoint;
    void Start()
    {
        player = GameObject.Find("Knight").transform;
        player.TryGetComponent<BossUI>(out BossUI bossUI);
        bUIReference = bossUI.UIReference;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRoom)
        {
            bUIReference.SetActive(true);
            Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);
            transform.LookAt(playerPos);
            if (canAttack) StartCoroutine(attack());
            else StartCoroutine(waitingToAttack());
        }
        else return;

    }
    private IEnumerator attack() 
    {
        Vector3 attackSpot = new Vector3(player.position.x, 0, player.position.z);
        GameObject area = Instantiate(areaMarker);
        area.transform.position = attackSpot;
        yield return new WaitForSeconds(attackDelay);
        BossFireBall fireball;
        fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        fireball.transform.LookAt(attackSpot);

        fireball.TryGetComponent<Rigidbody>(out Rigidbody rig);
        fireball.TryGetComponent<FireBall>(out FireBall fire);

        fire.dmg = dmg;
        Vector3 direction = (attackSpot - FirePoint.position).normalized;

        rig.AddForce(direction * shootForce, ForceMode.Impulse);
        
        yield return canAttack = false;
    }
    private IEnumerator waitingToAttack()
    {
        yield return new WaitForSeconds(25);
        yield return canAttack = true;
    }
}
