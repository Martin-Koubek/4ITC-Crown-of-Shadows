using TMPro;
using UnityEngine;

public class inventoryUI : MonoBehaviour
{
    public Inventory inventory;

    public bool hover = false;

    public GameObject StatWindow;

    public TextMeshProUGUI dmgText;

    void Start()
    {
        hover = true;       
    }

    void Update()
    {
    }
    public void OnCollisionEnter(Collision collision)
    {
        inventory.CurentWeapon.TryGetComponent<Sword>(out Sword sword);
        dmgText.text = "Dmg:" + sword.damage;
        StatWindow.SetActive(true);
    }
    private void OnCollisionExit(Collision collision)
    {
        hover = false;
        StatWindow.SetActive(false);
    }
}
