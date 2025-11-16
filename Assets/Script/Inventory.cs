using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Inventory : MonoBehaviour
{
    public GameObject consumable;
    public int consumableAmount = 0;
    public int MaxConsumables = 5;
    public TextMeshProUGUI amount;

    public GameObject RHand;
    public GameObject CurentWeapon;

    public Transform Storage;
    public Transform DropPoint;

    [SerializeField]
    List<GameObject> WeaponReferences = new List<GameObject>();
    public Image image;

    private PlayerController playerController;

    private void Awake()
    {
        amount.text = consumableAmount.ToString();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (consumable == null)
        {
            amount.text = "0";
        }

        else if (consumable.gameObject.TryGetComponent<Potion>(out Potion potion))
        {
            amount.text = consumableAmount.ToString();
            image.sprite = potion.SourceImage;
        }

        if (CurentWeapon != null && CurentWeapon.TryGetComponent<Sword>(out Sword sword))
        {
            HandleWeapon(sword);
        }



    }
    private void HandleWeapon(Sword sword)
    {
        foreach (var weapon in WeaponReferences)
        {
            weapon.SetActive(false);

            GameObject weaponToActivate = WeaponReferences.Find(w => w.name == sword.gameObject.name);

            if (weaponToActivate != null)
            {
                weaponToActivate.SetActive(true);
            }
            else Debug.Log("weapon model" + sword.gameObject.name + "nebyl nalezen v seznamu");
        }
        if (CurentWeapon != null)
        {
            if (sword.type == WeaponType.OneHanded)
            {
                playerController.oneHandedWeapon = true;
                playerController.twoHandedWeapon = false;
            }
            else if (sword.type == WeaponType.TwoHanded)
            {
                playerController.oneHandedWeapon = false;
                playerController.twoHandedWeapon = true;
            }
        }

    }
    public void DropWeapon()
    {
        CurentWeapon.gameObject.transform.SetParent(null);
        CurentWeapon.gameObject.transform.position = DropPoint.position;
        CurentWeapon.SetActive(true);
        CurentWeapon = null;
    }
}
