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
        bool toggle = false;
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.right.normalized * move.x + cameraTransform.forward.normalized * move.z;
        move.y = 0f;

        if(sprinting && !toggle)
        {
            toggle = true;
            Debug.Log(toggle + "sprint toggled on");
        }

        if (toggle)
        {
            Debug.Log("zacaloto");
            controller.Move(move * Time.deltaTime * sprintSpeed);
            animator.SetFloat(moveXAnimationParameterId, movement.x);
            animator.SetFloat(moveZAnimationParameterId, movement.y);
            Debug.Log("Funguje to");
        }
        else if (!toggle)
        {
            Debug.Log("Default");
            controller.Move(move * Time.deltaTime * playerSpeed);
            animator.SetFloat(moveXAnimationParameterId, movement.x);
            animator.SetFloat(moveZAnimationParameterId, movement.y / 2);
        }
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        //if(inputManager.PlayerDashed() && !Dashed)
        //{

        //}
    }
}
