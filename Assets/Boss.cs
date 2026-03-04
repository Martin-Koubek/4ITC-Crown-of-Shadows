using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int basicHealth = 100;
    private int _maxHealth;
    public float curentHealth = 100;
    [SerializeField]
    private float baseDmg = 5;
    public float dmg;
    public float shootForce = 50f;
    public float attackDelay = 10f;

    public TextMeshProUGUI health;
    public TextMeshProUGUI lvl;
    public Image healthFillBar;

    public Transform player;

    public float timeBetweenAttacks;
    public bool attacked;

    public GameObject areaMarker;
    public FireBall FireBall;

    public Transform FirePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Knight").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
    private IEnumerator attack() 
    {
        GameObject area = Instantiate(areaMarker);
        area.transform.position = player.position;
        yield return new WaitForSeconds(attackDelay);
        FireBall fireball;
        fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        fireball = Instantiate(FireBall, FirePoint.position, FirePoint.rotation);
        fireball.transform.LookAt(player);

        fireball.TryGetComponent<Rigidbody>(out Rigidbody rig);
        fireball.TryGetComponent<FireBall>(out FireBall fire);

        fire.dmg = dmg;
        fire.attackRange = attackRange;

        Vector3 direction = (player.position - FirePoint.position).normalized;

        rig.AddForce(direction * shootForce, ForceMode.Impulse);


        yield return null;
    }
}
