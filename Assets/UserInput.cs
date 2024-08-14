using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;
    public bool MoveLeft { get; private set; }
    public bool MoveRight { get; private set; }
    public bool JumpJustPressed { get; private set; }
    public bool JumpBeingHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool Run { get; private set; }
    public bool SuperJump { get; private set; }
    public bool Crouch { get; private set; }
    public bool Interact { get; private set; }
    public bool Transform { get; private set; }
    public bool Talk { get; private set; }
    public bool Pause { get; private set; }
    public string MoveLeftButton { get; private set; }
    public string MoveRightButton { get; private set; }
    public string JumpButton { get; private set; }
    public string RunButton { get; private set; }
    public string SuperJumpButton { get; private set; }
    public string CrouchButton { get; private set; }
    public string InteractButton { get; private set; }
    public string TransformButton { get; private set; }
    public string TalkButton { get; private set; }
    public string PauseButton { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _moveLeftAction;
    private InputAction _moveRightAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _superJumpAction;
    private InputAction _crouchAction;
    private InputAction _interactAction;
    private InputAction _transformAction;
    private InputAction _talkAction;
    private InputAction _pauseAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        _moveLeftAction = _playerInput.actions["move left"];
        _moveRightAction = _playerInput.actions["move right"];
        _jumpAction = _playerInput.actions["jump"];
        _runAction = _playerInput.actions["run"];
        _superJumpAction = _playerInput.actions["super jump"];
        _crouchAction = _playerInput.actions["crouch"];
        _interactAction = _playerInput.actions["interact"];
        _transformAction = _playerInput.actions["transform"];
        _talkAction = _playerInput.actions["talk"];
        _pauseAction = _playerInput.actions["pause"];
    }

    private void UpdateInputs()
    {
        MoveLeft = _moveLeftAction.IsPressed();
        MoveRight = _moveRightAction.IsPressed();
        JumpJustPressed = _jumpAction.WasPressedThisFrame();
        JumpBeingHeld = _jumpAction.IsPressed();
        JumpReleased = _jumpAction.WasReleasedThisFrame();
        Run = _runAction.IsPressed();
        SuperJump = _superJumpAction.WasPressedThisFrame();
        Crouch = _crouchAction.IsPressed();
        Interact = _interactAction.WasPressedThisFrame();
        Transform = _transformAction.WasPressedThisFrame();
        Talk = _talkAction.WasPressedThisFrame();
        Pause = _pauseAction.WasPressedThisFrame();

        MoveLeftButton = _moveLeftAction.GetBindingDisplayString();
        MoveRightButton = _moveRightAction.GetBindingDisplayString();
        JumpButton = _jumpAction.GetBindingDisplayString();
        RunButton = _runAction.GetBindingDisplayString();
        SuperJumpButton = _superJumpAction.GetBindingDisplayString();
        CrouchButton = _crouchAction.GetBindingDisplayString();
        InteractButton = _interactAction.GetBindingDisplayString();
        TransformButton = _transformAction.GetBindingDisplayString();
        TalkButton = _talkAction.GetBindingDisplayString();
        PauseButton = _pauseAction.GetBindingDisplayString();
    }
}
