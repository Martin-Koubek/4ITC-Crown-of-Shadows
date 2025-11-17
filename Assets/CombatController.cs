using UnityEngine;

public class CombatController : MonoBehaviour
{
    private InputManager inputManager;
    private Animator anim;
    private AnimatorStateInfo state;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0f;
    float maxComboDelay = 1f;

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
        if (state.normalizedTime > 0.99 && state.IsName("Hit1") || state.IsName("Hit1 1"))
        {
            if (anim.GetBool("Hit1"))
            {
                Debug.Log("Reset Hit1");
                anim.SetBool("Hit1", false);
            }
            
        }
        if (state.normalizedTime > 0.99 && state.IsName("Hit2") ||  state.IsName("Hit2 1"))
        {
            if (anim.GetBool("Hit2"))
            {
                Debug.Log("Reset Hit2");
                anim.SetBool("Hit2", false);
            }
        }
        if (state.normalizedTime > 0.99 && state.IsName("Hit3") || state.IsName("Hit3 1"))
        {
            if (anim.GetBool("Hit3"))
            {
                Debug.Log("Reset Hit3");
                anim.SetBool("Hit3", false);
                noOfClicks = 0;
            }
            //noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (attacked)
            {
                if (anim.GetBool("Armed") == true)
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
        if (noOfClicks == 1)
        {
            anim.SetBool("Hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks == 2 && state.normalizedTime > 0.7f && state.IsName("Hit1")|| noOfClicks == 2 && state.normalizedTime > 0.7f && state.IsName("Hit1 1"))
        {
            anim.SetBool("Hit2", true);
        }
        if (noOfClicks == 3 && state.normalizedTime > 0.7f && state.IsName("Hit2") || noOfClicks == 2 && state.normalizedTime > 0.7f && state.IsName("Hit2 1"))
        {
            anim.SetBool("Hit3", true);
        }


    }
}
