using System.Collections;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //stats + coliders
    public int basicHealth = 100;
    private int _maxHealth;
    public float curentHealth = 100;
    private int Lvl = 1;
    public bool hasBeenHit = false;
    private Collider coll;
    private EnemyRespawn spawner;

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

        player = GameObject.Find("Knight").transform;
        agent = GetComponent<NavMeshAgent>();

        spawner = GetComponentInParent<EnemyRespawn>();

        anim = GetComponent<Animator>();
        AnimatorMoveZId = Animator.StringToHash("MoveZ");
        AnimatorDeathIdle = Animator.StringToHash("Dead");

        coll = GetComponent<Collider>();

    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange) AttackPlayer();

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

        if(walkPointSet) agent.SetDestination(walkingPoint);

        Vector3 distanceToWalkPoint = transform.position - walkingPoint;

        if(distanceToWalkPoint.magnitude < 1f) walkPointSet=false;
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
        if(Physics.Raycast(player.transform.position, -transform.up, 5f, whatIsGround))
        {
            anim.SetFloat(AnimatorMoveZId, 1f);
            agent.SetDestination(player.position);
        }
        else Patroling();
       
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!attacked)
        {
            //attack code



            attacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        attacked=false;
    }
}
