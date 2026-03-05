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
    public bool stuned = false;

    [SerializeField]
    private float baseDmg = 5;

    public float dmg = 20;
    public float shootForce = 50f;
    public float attackDelay = 5f;

    public Transform player;
    public bool hasBeenHit;

    public float timeBetweenAttacks;
    public bool attacked;

    public GameObject bUIReference;
    public BossUI ui;

    public GameObject areaMarker;
    public GameObject area;
    public BossFireBall FireBall;

    public Transform FirePoint;

    //animace
    private Animator anim;
    private int AnimatorHitId;

    void Start()
    {
        player = GameObject.Find("Knight").transform;
        player.TryGetComponent<BossUI>(out BossUI bossUI);
        bUIReference = bossUI.UIReference;
        ui = bossUI;

        AnimatorHitId = Animator.StringToHash("cantAttack");
    }

    void Update()
    {
        ui.curHealth.text = curentHealth.ToString();
        ui.FillImage.fillAmount = curentHealth/_maxHealth;

        if (playerInRoom)
        {
            bUIReference.SetActive(true);
            Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(playerPos);
            if (canAttack && !stuned)  
            {
                AttackPlayer(); 
            }
            else return;
        }
        else return;

        if(curentHealth <= 0)
        {
            bUIReference.SetActive(false);
            Destroy(gameObject);
        }

    }

    private void AttackPlayer()
    {
        canAttack = false;
        Vector3 areaSpot = new Vector3(player.position.x, 0, player.position.z);
        area = Instantiate(areaMarker);
        area.transform.position = areaSpot;
        BossFireBall fireball;
        fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        fireball.TryGetComponent<Rigidbody>(out Rigidbody rig);
        fireball.TryGetComponent<BossFireBall>(out BossFireBall fire);
        fireball.transform.LookAt(areaSpot);

        fire.dmg = dmg;
        Vector3 direction = (areaSpot - FirePoint.position).normalized;

        rig.AddForce(direction * shootForce, ForceMode.Impulse);
    }

    public void KnockBack()
    {
        anim.SetTrigger(AnimatorHitId);
        stuned = true;
    }
    public void EndStun()
    {
        stuned = false;
        anim.ResetTrigger(AnimatorHitId);
    }

    public void StartAttackCooldown()
    {
        StartCoroutine(AttackCooldownRoutine());
    }

    // Samotný odpoèet
    private IEnumerator AttackCooldownRoutine()
    {
        // canAttack je v tuhle chvíli false (nastaveno pøi výstøelu)
        yield return new WaitForSeconds(attackDelay);
        canAttack = true; // Po uplynutí delaye boss mùže znovu útoèit
    }

}
