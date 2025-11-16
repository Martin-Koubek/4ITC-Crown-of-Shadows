using UnityEngine;

public class CombatController : MonoBehaviour
{
    private InputManager inputManager;
    private Animator anim;
    private AnimatorStateInfo state;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    private int _hit1Id;
    private int _hit2Id;
    private int _hit3Id;

    private void Start()
    {
        anim = GetComponent<Animator>();
        inputManager = InputManager.Instance;
        state = anim.GetCurrentAnimatorStateInfo(0);
    }

    void Update()
    {
        bool attacked = inputManager.GetPlayerAttack();
        Debug.Log(attacked);
        if (state.normalizedTime > 0.7 && state.IsName("Hit1"))
        {
            anim.SetBool("Hit1", false);
        }
        if (state.normalizedTime > 0.7 && state.IsName("Hit2"))
        {
            anim.SetBool("Hit2", false);
        }
        if (state.normalizedTime > 0.7 && state.IsName("Hit3"))
        {
            anim.SetBool("Hit3", false);
            noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (attacked)
            {
                OnClick();
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

        if (noOfClicks >= 2 && state.normalizedTime > 0.7f && state.IsName("Hit1"))
        {
            anim.SetBool("Hit1", false);
            anim.SetBool("Hit2", true);
        }
        if (noOfClicks >= 3 && state.normalizedTime > 0.7f && state.IsName("Hit2"))
        {
            anim.SetBool("Hit2", false);
            anim.SetBool("Hit3", true);
        }


    }
}
