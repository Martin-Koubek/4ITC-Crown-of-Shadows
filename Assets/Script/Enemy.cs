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
    private int Lvl = 1;
    public bool hasBeenHit = false;
    private Collider coll;

    //UI
    public TextMeshProUGUI health;
    public TextMeshProUGUI lvl;
    public Image healthFillBar;

    //navigation
    private NavMeshAgent agent;

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
        lvl.text = "Lvl:" + Lvl;

        _maxHealth = basicHealth * Lvl;
        curentHealth = _maxHealth;

        agent = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        AnimatorDeathIdle = Animator.StringToHash("Dead");
    }
    private void Update()
    {
        health.text = curentHealth + "/" + _maxHealth;

        healthFillBar.fillAmount = curentHealth / _maxHealth;

        if(curentHealth <= 0)
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
