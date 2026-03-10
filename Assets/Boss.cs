using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public bool playerInRoom;

    public int basicHealth = 100;
    private float _maxHealth = 200f; // Changed to float
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
    public GameObject screenManager;

    public GameObject areaMarker;
    public GameObject area;
    public BossFireBall FireBall;

    public Transform FirePoint;

    //animace
    private Animator anim;
    private int AnimatorHitId;

    void Start()
    {
        anim = GetComponent<Animator>(); // FIXED missing initialization
        
        GameObject p = GameObject.Find("Knight");
        if (p != null)
        {
            player = p.transform;
            if (player.TryGetComponent<BossUI>(out BossUI bossUI))
            {
                bUIReference = bossUI.UIReference;
                ui = bossUI;
            }
        }
        
        AnimatorHitId = Animator.StringToHash("cantAttack");
    }

    void Update()
    {
        if (ui != null && ui.curHealth != null && ui.FillImage != null)
        {
            ui.curHealth.text = curentHealth.ToString();
            ui.FillImage.fillAmount = curentHealth / _maxHealth;
        }

        if (playerInRoom)
        {
            if (bUIReference != null) bUIReference.SetActive(true);
            
            if (player != null)
            {
                Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(playerPos);
                if (canAttack && !stuned)  
                {
                    AttackPlayer(); 
                }
            }
        }

        if (curentHealth <= 0)
        {
            canAttack = false;
            if (player != null && player.TryGetComponent<VictoryScreenManager>(out VictoryScreenManager manager))
            {
                if (manager.menu != null) manager.menu.SetActive(true);
            }
            if (bUIReference != null) bUIReference.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void AttackPlayer()
    {
        canAttack = false;
        if (player == null || areaMarker == null || FireBall == null || FirePoint == null) return;
        
        Vector3 areaSpot = new Vector3(player.position.x, 0, player.position.z);
        area = Instantiate(areaMarker);
        area.transform.position = areaSpot;
        
        BossFireBall fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        
        if (fireball.TryGetComponent<Rigidbody>(out Rigidbody rig) && fireball.TryGetComponent<BossFireBall>(out BossFireBall fire))
        {
            fireball.transform.LookAt(areaSpot);
            fire.dmg = dmg;
            Vector3 direction = (areaSpot - FirePoint.position).normalized;
            rig.AddForce(direction * shootForce, ForceMode.Impulse);
        }
    }

    public void KnockBack()
    {
        if (anim != null) anim.SetTrigger(AnimatorHitId);
        stuned = true;
    }
    
    public void EndStun()
    {
        stuned = false;
        if (anim != null) anim.ResetTrigger(AnimatorHitId);
    }

    public void StartAttackCooldown()
    {
        StartCoroutine(AttackCooldownRoutine());
    }

    // Samotne odpocet (removed czech accents to prevent encoding issues)
    private IEnumerator AttackCooldownRoutine()
    {
        // canAttack je o tuhle chvili false (nastaveno pri vystrelu)
        yield return new WaitForSeconds(attackDelay);
        canAttack = true; // Po uplynuti delaye boss muze znovu utocit
    }
}
