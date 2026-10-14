using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour //будет все читать
{
    private static InputActionAsset _actions;
    
    private PlayerInput _playerInput;
    
    private InputAction _moveAction;
    private InputAction _fireAction;
    private InputAction _jumpAction;
    
    public static Vector2 MoveDirection { get; private set; }

    //public static event Action<Vector2> OnMove;
    public static event Action OnFire;
    public static event Action OnJump;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindActionMap("Player")["Move"];
        _fireAction = _playerInput.actions.FindActionMap("Player")["Attack"];
        _jumpAction = _playerInput.actions.FindActionMap("Player")["Jump"];
    }

    private void OnEnable()
    {//_fireAction.performed += _ => OnFire?.Invoke(); // _ - параметр, который мы не используем
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;
        _fireAction.performed += OnFireHandle;
        _jumpAction.performed += _ => OnJump?.Invoke();
    }
    //нажата кнопка? Мы идем куда-то
    private void OnFireHandle(InputAction.CallbackContext obj) => OnFire?.Invoke();
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        MoveDirection = ctx.ReadValue<Vector2>();
    }
    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        MoveDirection = Vector2.zero;
    }
    
    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _fireAction.performed -= OnFireHandle;
    }
}
