using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public LayerMask whatIsGround;

    private InputManager inputManager;
    private Inventory inventory;
    private PlayerController playerController;
    public PlayerStats playerStats;

    [SerializeField]
    private Transform _RayCastPoint;
    private RaycastHit hit;

    public Transform PlayerRef;
    public Transform grondCheck;

    [SerializeField] public MapGenerator mapGenerator;

    [SerializeField]
    private float _hitRange;

    public bool toReset;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        inventory = GetComponent<Inventory>();
        playerController = GetComponent<PlayerController>();
        toReset = false;
    }
    private void Update()
    {
        /*if (!Physics.Raycast(grondCheck.position, -transform.up, 5f, whatIsGround) && grondCheck.position.y < -10f)
        {
            ResetPlayer(PlayerRef.gameObject);
        }*/
        if (toReset)
        {
            ResetPlayer(PlayerRef.gameObject);
        }

        bool interacted = inputManager.GetPlayerInteract();
        bool Healed = inputManager.GetPlayerHeal();
        Ray ray = new Ray(_RayCastPoint.position, _RayCastPoint.forward);

        if (interacted)
        {
            if (Physics.Raycast(ray, out hit, _hitRange))
            {
                if (hit.collider.gameObject.TryGetComponent<DoorSC>(out DoorSC door))
                {
                    Debug.Log("Toto jsou dveøe");
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

                else if (hit.collider.gameObject.GetComponent<Exit>())
                {
                    StartCoroutine(DestoryFloor());
                    mapGenerator.currentLevel++;
                }
                else if (hit.collider.gameObject.TryGetComponent<Chest>(out Chest chest))
                {
                    chest.OpenChest();
                }

                else return;

            }
        }

        if (Healed)
        {
            if (inventory.consumableAmount != 0)
            {
                inventory.consumableAmount--;
                Heal();
            }
            else return;
        }
    }

    private void Heal()
    {
        TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.curentHealth += 25;
        if (stats.curentHealth > stats._maxHealth)
        {
            stats.curentHealth = stats._maxHealth;
        }
        else return;
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

    private IEnumerator DestoryFloor()
    {
        foreach (Transform child in mapGenerator.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        mapGenerator.spawnedRooms.Clear();
        mapGenerator.spawnedFloors.Clear();
        yield return null;
        mapGenerator.NewFloorGen();
        yield return new WaitForEndOfFrame();
        ResetPlayer(PlayerRef.gameObject);
    }
    public void ResetPlayer(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        cc.enabled = false;
        player.transform.position = new Vector3(0,1,0);
        cc.enabled = true;
        toReset = false;
    }
}
