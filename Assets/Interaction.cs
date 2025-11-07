using System.Runtime.CompilerServices;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private InputManager inputManager;
    private RaycastHit hit;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void Update()
    {
        bool interacted = inputManager.GetPlayerInteract();

        if (interacted)
        {
        
        }

    }
}
