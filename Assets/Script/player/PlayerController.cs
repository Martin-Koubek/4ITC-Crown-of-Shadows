using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
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
    bool armed = false;
    [SerializeField]
    bool oneHandedWeapon = false;
    [SerializeField]
    bool twoHandedWeapon = false;
    [SerializeField]
    bool dualWield = false;

   


    //private bool Dashed;
    private InputManager inputManager;
    [SerializeField]
    private Transform cameraTransform;
    //animations
    [Header("Animator")]
    private Animator animator;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;
    int armedAnimationParameterId;
    int onehandedAnimationParameterId;
    int twoHandedAnimationParameterId;
    int dualWieldAnimationParameterId;
    

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
        dualWieldAnimationParameterId = Animator.StringToHash("dualWield");
    }

    private void Update()
    {
        bool inventoryOpen = inputManager.GetPlayerInventory();
        bool menuOpen = inputManager.GetPlayerMenu();
        bool sprinting = inputManager.GetPlayetSprint();
        Debug.Log(sprinting);
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.right.normalized * move.x + cameraTransform.forward.normalized * move.z;
        move.y = 0f;


        if (grounded && playerVelocity.y < 0f)
        {
            playerVelocity.y = 0f;
        }
        if(inventoryOpen == true && invOpen == false)
        {
            invOpen = true;
            pointer.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventory.gameObject.SetActive(true);
        }
        else if(inventoryOpen == true && invOpen == true)
        {
            invOpen = false;
            pointer.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventory.gameObject.SetActive(false);
        }
        if (menuOpen == true && menOpen == false)
        {
            menOpen = true;
            pointer.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Menu.gameObject.SetActive(true);
        }
        else if (menuOpen == true && menOpen == true)
        {
            menOpen = false;
            pointer.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Menu.gameObject.SetActive(false);
        }
        if(armed == false)
        {
            animator.SetBool(armedAnimationParameterId, false);

            if (sprinting)
            {
                Debug.Log("zacaloto");
                controller.Move(move * Time.deltaTime * sprintSpeed);
                animator.SetFloat(moveXAnimationParameterId, movement.x);
                animator.SetFloat(moveZAnimationParameterId, movement.y);
                Debug.Log("Funguje to");
            }
            else if (!sprinting)
            {
                Debug.Log("Default");
                controller.Move(move * Time.deltaTime * playerSpeed);
                animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
                animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
            }
        }
        else if(armed == true)
        {
            animator.SetBool(armedAnimationParameterId, true);

            if (oneHandedWeapon == true)
            {
                animator.SetBool(onehandedAnimationParameterId, true);
                animator.SetBool(twoHandedAnimationParameterId, false);
                animator.SetBool(dualWieldAnimationParameterId, false);

                if (sprinting)
                {
                    Debug.Log("zacaloto");
                    controller.Move(move * Time.deltaTime * sprintSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x);
                    animator.SetFloat(moveZAnimationParameterId, movement.y);
                    Debug.Log("Funguje to");
                }
                else if (!sprinting)
                {
                    Debug.Log("Default");
                    controller.Move(move * Time.deltaTime * playerSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
                    animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
                }
            }
            else if (twoHandedWeapon == true)
            {
                animator.SetBool(onehandedAnimationParameterId, false);
                animator.SetBool(twoHandedAnimationParameterId, true);
                animator.SetBool(dualWieldAnimationParameterId, false);

                if (sprinting)
                {
                    Debug.Log("zacaloto");
                    controller.Move(move * Time.deltaTime * sprintSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x);
                    animator.SetFloat(moveZAnimationParameterId, movement.y);
                    Debug.Log("Funguje to");
                }
                else if (!sprinting)
                {
                    Debug.Log("Default");
                    controller.Move(move * Time.deltaTime * playerSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
                    animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
                }
            }
            else if (dualWield == true)
            {
                animator.SetBool(onehandedAnimationParameterId, false);
                animator.SetBool(twoHandedAnimationParameterId, false);
                animator.SetBool(dualWieldAnimationParameterId, true);

                if (sprinting)
                {
                    Debug.Log("zacaloto");
                    controller.Move(move * Time.deltaTime * sprintSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x);
                    animator.SetFloat(moveZAnimationParameterId, movement.y);
                    Debug.Log("Funguje to");
                }
                else if (!sprinting)
                {
                    Debug.Log("Default");
                    controller.Move(move * Time.deltaTime * playerSpeed);
                    animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
                    animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
                }
            }
        }

        if (sprinting)
        {
            Debug.Log("zacaloto");
            controller.Move(move * Time.deltaTime * sprintSpeed);
            animator.SetFloat(moveXAnimationParameterId, movement.x);
            animator.SetFloat(moveZAnimationParameterId, movement.y);
            Debug.Log("Funguje to");
        }
        else if (!sprinting)
        {
            Debug.Log("Default");
            controller.Move(move * Time.deltaTime * playerSpeed);
            animator.SetFloat(moveXAnimationParameterId, movement.x / 2);
            animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        //if(inputManager.PlayerDashed() && !Dashed)
        //{

        //}
    }
}
