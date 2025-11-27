using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //stats + coliders
    public int basicHealth = 100;
    private int _maxHealth;
    public float curentHealth = 100;
    [SerializeField]
    private float baseDmg = 5;
    public float dmg;
    private int Lvl = 1;
    public bool hasBeenHit = false;
    private Collider coll;
    private EnemyRespawn spawner;
    public bool isMage;
    public GameObject FireBall;
    public Transform FirePoint;

    public bool isAttacking = false;

    //UI
    public TextMeshProUGUI health;
    public TextMeshProUGUI lvl;
    public Image healthFillBar;

    //navigation
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //patroling
    public Vector3 walkingPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    public bool attacked;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //animator
    private Animator anim;
    private int AnimatorDeathIdle;
    private int AnimatorMoveZId;
    private int AnimatorAttackId;
    private int AnimatorIsMage;



    private IEnumerator DestroyAfterDelay(Collider collider)
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void Awake()
    {
        lvl.text = "Lvl:" + Lvl;

        _maxHealth = basicHealth * Lvl;
        curentHealth = _maxHealth;
        dmg = baseDmg * Lvl;

        player = GameObject.Find("Knight").transform;

        agent = GetComponent<NavMeshAgent>();

        spawner = GetComponentInParent<EnemyRespawn>();

        anim = GetComponent<Animator>();

        AnimatorIsMage = Animator.StringToHash("isMage");
        AnimatorAttackId = Animator.StringToHash("attacked");
        AnimatorMoveZId = Animator.StringToHash("MoveZ");
        AnimatorDeathIdle = Animator.StringToHash("Dead");

        coll = GetComponent<Collider>();

        if(isMage)anim.SetBool(AnimatorIsMage, true);
        else anim.SetBool(AnimatorIsMage, false);
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
        health.text = curentHealth + "/" + _maxHealth;

        healthFillBar.fillAmount = curentHealth / _maxHealth;

        if (curentHealth <= 0)
        {
            anim.SetBool(AnimatorDeathIdle, true);
            Die();

        }


    }

    private void Die()
    {
        StartCoroutine(DestroyAfterDelay(coll));
        spawner.isDead = true;
    }

    private void Move()
    {

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkingPoint);

        Vector3 distanceToWalkPoint = transform.position - walkingPoint;

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }
    private void SearchWalkPoint()
    {

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkingPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //walkPointSet = true;
        if (Physics.Raycast(walkingPoint, -transform.up, 5f, whatIsGround))
        {
            anim.SetFloat(AnimatorMoveZId, 0.5f);
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        if (Physics.Raycast(player.transform.position, -transform.up, 5f, whatIsGround))
        {
            anim.SetFloat(AnimatorMoveZId, 1f);
            Vector3 playerView = new Vector3(player.position.x, player.position.y + 2, player.position.z);
            transform.LookAt(playerView);
            agent.SetDestination(player.position);
        }

    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!attacked && player.TryGetComponent<PlayerStats>(out PlayerStats playerS))
        {
            if (!playerS.hasBeenHit)
            {
                if (!isMage)
                {
                    anim.SetTrigger(AnimatorAttackId);
                    attacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
                else
                {
                    //vystøelí fireBall
                    anim.SetTrigger(AnimatorAttackId);

                    GameObject fireball;
                    fireball = Instantiate(FireBall, FirePoint);
                    fireball.TryGetComponent<Rigidbody>(out Rigidbody rig);
                    rig.AddForce(player.transform.position, ForceMode.Impulse);

                    attacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
            else return;
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }

    [System.Obsolete]
    public void StartAttack()
    {
        isAttacking = true;
        foreach (var player in FindObjectsOfType<PlayerStats>())
        {
            player.hasBeenHit = false;
        }
    }

    [System.Obsolete]
    public void StopAttack()
    {
        isAttacking = false;
        foreach (var player in FindObjectsOfType<PlayerStats>())
        {
            player.hasBeenHit = false;
            anim.ResetTrigger(AnimatorAttackId);
        }
    }
}
