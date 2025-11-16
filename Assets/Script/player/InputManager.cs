using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;


    public static InputManager Instance
    {
        get { return instance; }
    }
    [SerializeField]
    private PlayerControls playerControls;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this.gameObject); }
        else instance = this;
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public bool GetPlayetSprint()
    {
        return playerControls.Player.Sprint.inProgress;
    }
        
    public bool GetPlayerInventory()
    {
        return playerControls.Player.Inventory.triggered;
    }

    public bool GetPlayerMenu()
    {
        return playerControls.Player.Pause.triggered;
    }
    public bool GetPlayerAttack()
    {
        return playerControls.Player.Attack.triggered;
    }
    public bool GetPlayerBlock_HeavyAttack()
    {
        return playerControls.Player.BlockHeavyAttack.inProgress;
    }
    public bool GetPlayerInteract()
    {
        return playerControls.Player.Interact.triggered;
    }
            


    

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }
   

    //public bool PlayerDashed()
    //{
    //    return playerControls.Player.Dash.triggered;
    //}
}
