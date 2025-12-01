using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Hittabel))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 3f;
    [SerializeField]
    private float dashDistance = 5f;
    [SerializeField]
    private float rotationSpeed = .8f;
    [SerializeField]
    private float sprintSpeed = 6f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private Canvas inventory;
    [SerializeField]
    private Canvas pointer;
    [SerializeField]
    private Canvas Menu;
    private bool grounded = true;
    private bool invOpen = false;
    private bool menOpen = false;
    private Vector3 playerVelocity;
    private CharacterController controller;

    //Weapon State
    [SerializeField]
    public bool armed = false;
    [SerializeField]
    public bool oneHandedWeapon = false;
    [SerializeField]
    public bool twoHandedWeapon = false;

    private InputManager inputManager;
    [SerializeField]
    private Transform cameraTransform;
    public CinemachineCamera camera;

    //animations
    [Header("Animator")]
    private Animator animator;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;
    int armedAnimationParameterId;
    int onehandedAnimationParameterId;
    int twoHandedAnimationParameterId;


    private void Awake()
    {
        armed = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = gameObject.GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        moveXAnimationParameterId = Animator.StringToHash("moveX");
        moveZAnimationParameterId = Animator.StringToHash("moveZ");
        armedAnimationParameterId = Animator.StringToHash("Armed");
        onehandedAnimationParameterId = Animator.StringToHash("1handed");
        twoHandedAnimationParameterId = Animator.StringToHash("2handed");
    }

    private void Update()
    {
        bool inventoryOpen = inputManager.GetPlayerInventory();
        bool menuOpen = inputManager.GetPlayerMenu();
        bool sprinting = inputManager.GetPlayetSprint();

        bool attacked = inputManager.GetPlayerAttack();

        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.right.normalized * move.x + cameraTransform.forward.normalized * move.z;
        move.y = 0f;

        if (grounded && playerVelocity.y < 0f)
        {
            playerVelocity.y = 0f;
        }
        if (inventoryOpen == true && invOpen == false)
        {
            invOpen = true;
            Time.timeScale = 0f;
            pointer.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventory.gameObject.SetActive(true);
        }
        else if (inventoryOpen == true && invOpen == true)
        {
            invOpen = false;
            Time.timeScale = 1f;
            pointer.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventory.gameObject.SetActive(false);
        }
        if (menuOpen == true)
        {
            OpenMenu();
        }
        else if (menuOpen == true)
        {
            OpenMenu();
        }
        if (armed == false)
        {
            animator.SetBool(armedAnimationParameterId, false);

            if (sprinting)
            {
                controller.Move(move * Time.deltaTime * sprintSpeed);
                animator.SetFloat(moveXAnimationParameterId, movement.x);
                animator.SetFloat(moveZAnimationParameterId, movement.y);
            }
            else if (!sprinting)
            {
                controller.Move(move * Time.deltaTime * playerSpeed);
                animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
                animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
            }
        }
        else if (armed == true)
        {
            animator.SetBool(armedAnimationParameterId, true);

            if (oneHandedWeapon == true)
            {
                animator.SetBool(onehandedAnimationParameterId, true);
                animator.SetBool(twoHandedAnimationParameterId, false);


                if (sprinting)
                {
                    controller.Move(move * Time.deltaTime * sprintSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x);
                    animator.SetFloat(moveZAnimationParameterId, movement.y);
                }
                else if (!sprinting)
                {
                    controller.Move(move * Time.deltaTime * playerSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
                    animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
                }
            }
            else if (twoHandedWeapon == true)
            {
                animator.SetBool(onehandedAnimationParameterId, false);
                animator.SetBool(twoHandedAnimationParameterId, true);


                if (sprinting)
                {
                    controller.Move(move * Time.deltaTime * sprintSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x);
                    animator.SetFloat(moveZAnimationParameterId, movement.y);
                }
                else if (!sprinting)
                {
                    controller.Move(move * Time.deltaTime * playerSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
                    animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
                }
            }
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void OpenMenu()
    {
        if (menOpen == false)
        {
            menOpen = true;
            Time.timeScale = 0f;
            pointer.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Menu.gameObject.SetActive(true);
        }
        else if (menOpen == true)
        {
            menOpen = false;
            Time.timeScale = 1f;
            pointer.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Menu.gameObject.SetActive(false);
        }
    }

}



