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
    private int Lvl = 1;
    public bool hasBeenHit = false;
    public float shootForce = 50f;

    public TextMeshProUGUI health;
    public TextMeshProUGUI lvl;
    public Image healthFillBar;

    public Transform player;

    public float timeBetweenAttacks;
    public bool attacked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
