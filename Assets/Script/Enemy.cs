using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    private int Lvl = 1;
    public bool hasBeenHit = false;
    private Collider coll;

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
}
