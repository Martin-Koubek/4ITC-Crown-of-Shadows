using System.Collections;
using TMPro;
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
    public float shootForce = 50f;
    private Collider coll;
    private EnemyRespawn spawner;
    public bool isMage;
    public FireBall FireBall;
    public Transform FirePoint;
    public bool cantAttack;

    public bool isAttacking = false;

    //UI
    public TextMeshProUGUI health;
    public TextMeshProUGUI lvl;
    public Image healthFillBar;

    //navigation
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, wall;

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
    private int AnimatorHitId;



    private IEnumerator DestroyAfterDelay(Collider collider)
    {
        yield return new WaitForSeconds(5f);
        Destroy(this);
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
        AnimatorHitId = Animator.StringToHash("cantAttack");
        AnimatorDeathIdle = Animator.StringToHash("Dead");

        coll = GetComponent<Collider>();

        if (isMage) anim.SetBool(AnimatorIsMage, true);
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
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
        health.text = curentHealth + "/" + _maxHealth;

        healthFillBar.fillAmount = curentHealth / _maxHealth;

        if (curentHealth <= 0)
        {
            /*anim.SetBool(AnimatorDeathIdle, true);
            Die();*/
            Destroy(gameObject);
            spawner.Respawn();
        }


    }

    private void Die()
    {
        spawner.isDead = true;
        StartCoroutine(DestroyAfterDelay(coll));
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
        if (Physics.Raycast(transform.position, player.position, sightRange, wall) || Physics.Raycast(transform.position, player.position, sightRange, wall))
        {
            Patroling();
        }
        else
        {
            if (Physics.Raycast(player.transform.position, -transform.up, 5f, whatIsGround))
            {
                anim.SetFloat(AnimatorMoveZId, 1f);
                transform.LookAt(player);
                agent.SetDestination(player.position);
            }
        }


    }
    private void AttackPlayer()
    {
        Vector3 distanceToPlayer = transform.position - player.position;
        if (isMage && Physics.Raycast(transform.position, player.position, attackRange, wall))
        {
            Patroling();
        }

        if (distanceToPlayer.magnitude < attackRange) agent.SetDestination(transform.position);

        if (!attacked && player.TryGetComponent<PlayerStats>(out PlayerStats playerS) && !cantAttack)
        {
            if (!playerS.PlayerBeenHit)
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
                    attacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
            else return;
        }
    }

    private void CastFireBall()
    {
        FireBall fireball;
        fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        fireball.transform.LookAt(player);

        fireball.TryGetComponent<Rigidbody>(out Rigidbody rig);
        fireball.TryGetComponent<FireBall>(out FireBall fire);

        fire.dmg = dmg;
        fire.attackRange = attackRange;

        Vector3 direction = (player.position - FirePoint.position).normalized;

        rig.AddForce(direction * shootForce, ForceMode.Impulse);
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
            player.PlayerBeenHit = false;
        }
    }

    [System.Obsolete]
    public void StopAttack()
    {
        isAttacking = false;
        foreach (var player in FindObjectsOfType<PlayerStats>())
        {
            player.PlayerBeenHit = false;
            anim.ResetTrigger(AnimatorAttackId);
        }
    }
    public void NockBack()
    {
        anim.SetTrigger(AnimatorHitId);
        cantAttack = true;
    }

    public void EndStun()
    {
        cantAttack = false;
        anim.ResetTrigger(AnimatorHitId);
    }
}
