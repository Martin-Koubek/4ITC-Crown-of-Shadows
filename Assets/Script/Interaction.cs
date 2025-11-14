using UnityEngine;

public class Interaction : MonoBehaviour
{
    private InputManager inputManager;
    private Inventory inventory;
    [SerializeField]
    private Transform _RayCastPoint;
    private RaycastHit hit;
    [SerializeField]
    private float _hitRange;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        inventory = GetComponent<Inventory>();
    }
    private void Update()
    {
        bool interacted = inputManager.GetPlayerInteract();
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
                        Destroy(potion);
                    }
                }
                else return;

            }
        }

    }
}
