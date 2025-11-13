using System.Runtime.CompilerServices;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField]
    private Transform _RayCastPoint;
    private RaycastHit hit;
    [SerializeField]
    private float _hitRange;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void Update()
    {
        bool interacted = inputManager.GetPlayerInteract();
        Ray ray = new Ray(_RayCastPoint.position, _RayCastPoint.forward);
        if (interacted)
        {
            Debug.Log("interacted");
            Physics.Raycast(ray, out hit, _hitRange);
            if (hit.collider.gameObject.TryGetComponent<DoorSC>(out DoorSC door))
            {
                if (door.isOpen == false)
                {
                    door.isOpen = true;
                    Debug.Log("DoorOpend");
                }
                else
                {
                    door.isOpen = false;
                    Debug.Log("DoorClosed");
                }
            }
        }

    }
}
