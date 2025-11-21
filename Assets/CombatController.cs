using UnityEngine;

public class CombatController : MonoBehaviour
{
    private InputManager inputManager;
    private Animator anim;
    private AnimatorStateInfo state;
    public float cooldownTime = 1f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0f;
    float maxComboDelay = 1f;

    public bool isAttacking = false;

    private int _hit1Id;
    private int _hit2Id;
    private int _hit3Id;

    private void Start()
    {
        anim = GetComponent<Animator>();
        inputManager = InputManager.Instance;

    }

    void Update()
    {
        Debug.Log(noOfClicks);
        state = anim.GetCurrentAnimatorStateInfo(0);

        bool attacked = inputManager.GetPlayerAttack();

        Debug.Log(attacked);
        if (state.normalizedTime > 0.99)
        {
            if (state.IsName("Hit1") || state.IsName("Hit1 1"))
            {
                if (anim.GetBool("Hit1"))
                {
                    anim.SetBool("Hit1", false);
                }
            }


        }
        if (state.normalizedTime > 0.99)
        {
            if (state.IsName("Hit2") || state.IsName("Hit2 1"))
            {
                if (anim.GetBool("Hit2"))
                {
                    anim.SetBool("Hit2", false);
                }
            }

        }
        if (state.normalizedTime > 0.99)
        {
            if (state.IsName("Hit3") || state.IsName("Hit3 1"))
            {
                if (anim.GetBool("Hit3"))
                {
                    anim.SetBool("Hit3", false);
                    noOfClicks = 0;
                }
            }
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (anim.GetBool("Armed"))
            {
                if (attacked)
                {
                    OnClick();
                    nextFireTime = Time.time + cooldownTime;
                }

            }
        }

    }
    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks >= 1)
        {
            anim.SetBool("Hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2)
        {
            if (state.IsName("Hit1") || state.IsName("Hit1 1"))
            {
                anim.SetBool("Hit2", true);
                anim.SetBool("Hit1", false);
            }
        }
        if (noOfClicks >= 3)
        {
            if (state.IsName("Hit2") || state.IsName("Hit2 1"))
            {
                anim.SetBool("Hit3", true);
                anim.SetBool("Hit2", false);
                anim.SetBool("Hit1", false);
            }
        }


    }
    [System.Obsolete]
    public void StartAttack()
    {
        isAttacking = true;
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            enemy.hasBeenHit = false;
        }
    }

    [System.Obsolete]
    public void StopAttack()
    {
        isAttacking=false;
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            enemy.hasBeenHit = false;
        }
    }
}
