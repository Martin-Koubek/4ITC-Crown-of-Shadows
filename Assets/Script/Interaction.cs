using System;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private InputManager inputManager;
    private Inventory inventory;
    private PlayerController playerController;
    private PlayerStats playerStats;

    [SerializeField]
    private Transform _RayCastPoint;
    private RaycastHit hit;

    [SerializeField]
    private float _hitRange;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        inventory = GetComponent<Inventory>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        bool interacted = inputManager.GetPlayerInteract();
        bool Healed = inputManager.GetPlayerHeal();
        Ray ray = new Ray(_RayCastPoint.position, _RayCastPoint.forward);

        if (interacted)
        {
            if (Physics.Raycast(ray, out hit, _hitRange))
            {
                if (hit.collider.gameObject.TryGetComponent<DoorSC>(out DoorSC door))
                {
                    Debug.Log("Toto jsou dve¯e");
                    if (!door.isOpen)
                    {
                        door.isOpen = true;
                    }
                    else
                    {
                        door.isOpen = false;
                    }
                }

                else if (hit.collider.gameObject.TryGetComponent<Potion>(out Potion potion))
                {
                    if (inventory.MaxConsumables == inventory.consumableAmount)
                    {
                        return;
                    }
                    else
                    {
                        Debug.Log("toto je poù·k");
                        inventory.consumable = potion.gameObject;
                        inventory.consumableAmount ++;
                        potion.gameObject.SetActive(false);
                        potion.gameObject.transform.SetParent(inventory.Storage);
                    }
                }

                else if (hit.collider.gameObject.TryGetComponent<Sword>(out Sword sword))
                {
                    if (inventory.CurentWeapon == null)
                    {
                        Debug.Log("Sebral jsem meË");
                        PickUp(sword);
                    }
                    else if(inventory.CurentWeapon != null)
                    {
                        inventory.DropWeapon();
                        PickUp(sword);
                        
                    }
                }
                else return;

            }
        }

        if (Healed)
        {
            if(inventory.consumableAmount != 0)
            {
                inventory.consumableAmount --;
                Heal();
            }
        }
    }

    private void Heal()
    {
    }

    private void PickUp(Sword sword)
    {
        inventory.CurentWeapon = sword.gameObject;
        playerController.armed = true;
        sword.gameObject.SetActive(false);
        sword.gameObject.transform.SetParent(inventory.Storage);
        sword.gameObject.transform.position = inventory.Storage.position;
    }
}
