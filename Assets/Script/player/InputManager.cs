using UnityEngine;

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

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }
   

    //public bool PlayerDashed()
    //{
    //    return playerControls.Player.Dash.triggered;
    //}
}
