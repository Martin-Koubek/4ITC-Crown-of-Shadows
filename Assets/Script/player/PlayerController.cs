using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private float _sprintSpeed;
    private bool _sprintTriggered;
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();

        _playerInputController.OnSprintButtonPressed += SprintButtonPressed;
    }
    private void FixedUpdate()
    {
        Vector3 positionChange = new Vector3(
            _playerInputController.MovementInputVector.x,
            0,
            _playerInputController.MovementInputVector.y) * Time.deltaTime * _speed;

        transform.position += positionChange;
    }

    private void SprintButtonPressed()
    {
        _sprintTriggered = true;
    }
}
