using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //stats + coliders
    public int Health = 100;
    private int Lvl = 1;
    public bool hasBeenHit = false;
    private Collider coll;

    //navigation
    public NavMeshAgent agent;
    public Transform Player;
    public LayerMask whatIsGround, whatIsplayer;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float inSightRange, inAttackRange;
    public bool playerInSightRange, playerInAttackRange;

    //animator
    private Animator anim;
    private int AnimatorDeathIdle;


    private IEnumerator DestroyAfterDelay(Collider collider)
    {
        yield return new WaitForSeconds(0.5f);
        collider.isTrigger = true;

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);

    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        AnimatorDeathIdle = Animator.StringToHash("Dead");
    }
    private void Update()
    {
        Debug.Log(Health);
        if(Health <= 0)
        {
            anim.SetBool(AnimatorDeathIdle, true);
            Die();

        }
    }

    private void Die()
    {
        StartCoroutine(DestroyAfterDelay(coll));
    }

    private void Move()
    {
        
    }
}
