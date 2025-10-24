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
    private bool grounded = true;
    private Vector3 playerVelocity;
    private CharacterController controller;

    //private bool Dashed;
    private InputManager inputManager;
    [SerializeField]
    private Transform cameraTransform;
    [Header("Animator")]
    private Animator animator;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;
    

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = gameObject.GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        moveXAnimationParameterId = Animator.StringToHash("moveX");
        moveZAnimationParameterId = Animator.StringToHash("moveZ");
    }

    private void Update()
    {
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
