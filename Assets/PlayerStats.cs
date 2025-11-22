using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private int _maxHealth = 100;
    public int curentHealth;

    public int defence = 0;

    public TextMeshProUGUI health;
    public Image healthFillBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curentHealth = _maxHealth;
    }

    void Update()
    {
        health.text = curentHealth + "/" + _maxHealth;
        healthFillBar.fillAmount = curentHealth / 100;
    }
}
