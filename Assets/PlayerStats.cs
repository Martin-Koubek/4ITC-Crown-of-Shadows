using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int _maxHealth = 100;
    public float curentHealth;

    public bool PlayerBeenHit = false;

    public int defence = 0;

    public TextMeshProUGUI health;
    public Image healthFillBar;

    public Canvas DeathScreen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curentHealth = _maxHealth;
    }

    void Update()
    {
        health.text = curentHealth + "/" + _maxHealth;
        healthFillBar.fillAmount = curentHealth / _maxHealth;
    }
}
