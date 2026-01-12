using System;
using System.Collections;
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

    [SerializeField] private MapGenerator mapGenerator;

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
                        inventory.consumableAmount++;
                        potion.gameObject.SetActive(false);
                        potion.gameObject.transform.SetParent(inventory.Storage);
                    }
                }

                else if (hit.collider.gameObject.TryGetComponent<Sword>(out Sword sword))
                {
                    if (inventory.CurentWeapon == null)
                    {
                        PickUp(sword);
                    }
                    else if (inventory.CurentWeapon != null)
                    {
                        inventory.DropWeapon();
                        PickUp(sword);

                    }
                }
                else if (hit.collider.gameObject.TryGetComponent<EndTutorial>(out EndTutorial end))
                {
                    end.LoadLevel("MainGame");
                }

                else if (hit.collider.gameObject.GetComponent<exit>())
                {
                    Debug.Log("Exit Pressed");
                    mapGenerator.resetPlayer();
                    mapGenerator._currentLevel++;
                    StartCoroutine(DestoruFloor());
                    StartCoroutine(mapGenerator.NewFloorGenerator());

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

        if (sword.type == WeaponType.TwoHanded)
        {
            inventory.RHSlot.sprite = sword.icon;
            inventory.LHSlot.sprite = sword.icon;
            inventory.RHSlot.color = Color.white;
            inventory.LHSlot.color = Color.white;
        }
        else if (sword.type == WeaponType.OneHanded)
        {
            inventory.RHSlot.sprite = sword.icon;
            inventory.LHSlot.sprite = null;
            inventory.RHSlot.color = Color.white;
            inventory.LHSlot.color = inventory.defColor;
        }
    }

    private IEnumerator DestoruFloor()
    {
        for (int i = 1; i < mapGenerator.rooms.Count; i++)
        {
            Destroy(mapGenerator.rooms[i]);
            mapGenerator.rooms[i] = null;
            yield return new WaitForEndOfFrame();
        }

    }
}
