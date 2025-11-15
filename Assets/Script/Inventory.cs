using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public GameObject consumable;
    public int consumableAmount = 0;
    public int MaxConsumables = 5;
    public TextMeshProUGUI amount;
    public GameObject RHand;
    public GameObject LHand;
    [SerializeField]
    List<GameObject>WeaponReferences = new List<GameObject>();
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
        else if (RHand.gameObject.TryGetComponent<Sword>(out Sword sword))
        {
            for (int i = 0; i < WeaponReferences.Count; i++)
            {
                if (WeaponReferences[i].name.ToString() == sword.type)
                {
                    Debug.Log("Sword Found");
                    WeaponReferences[i].SetActive(true);
                }
                else i++;
            }
        }
        else { return; }
    }
}
