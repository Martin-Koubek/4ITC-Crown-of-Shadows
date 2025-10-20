using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2f;
    [SerializeField]
    private float dashDistance = 5f;
    [SerializeField]
    private float rotationSpeed = .8f;

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
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.right.normalized * move.x + cameraTransform.forward.normalized * move.z;
        move.y = 0f;    
        controller.Move(move * Time.deltaTime * playerSpeed);

        animator.SetFloat(moveXAnimationParameterId, movement.x);
        animator.SetFloat(moveZAnimationParameterId, movement.y);


        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //if (movement != Vector2.zero)
        //{
        //    float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        //    Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        //}

        //if(inputManager.PlayerDashed() && !Dashed)
        //{

        //}
    }
}
