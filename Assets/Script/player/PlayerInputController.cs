using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public event Action OnSprintButtonPressed;
    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
    }
    private void OnSprint(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnSprintButtonPressed?.Invoke();
        }
    }
}
