using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject consumable;
    public int consumableAmount = 0;
    public int MaxConsumables = 5;
    public TextMeshProUGUI amount;
    public GameObject RHand;
    public GameObject LHand;
    public Image image;

    private void Awake()
    {
        amount.text = consumableAmount.ToString();
    }

    private void Update()
    {
        if (consumable == null) { amount.text = "0"; }
        else if (consumable.gameObject.TryGetComponent<Potion>(out Potion potion))
        {
            amount.text = consumableAmount.ToString();
            image.sprite = potion.SourceImage;
        }
        else { return; }
    }
}
