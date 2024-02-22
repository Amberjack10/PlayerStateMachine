using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    // 모든 State는 StateMachine과 역참조를 할 것!
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundedData groundedData;

    PlayerInputActions.PlayerActions playerActions;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        this.stateMachine = playerStateMachine;
        groundedData = stateMachine.Player.Data.GroundedData;
        playerActions = stateMachine.Player.Input.PlayerActions;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMoveInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    // InputAction에 이벤트를 구독하는 것과 유사하게 Callback을 걸어준다.
    // 이를 Enter와 Exit에 걸어두어 State 진입 시 InputAction을 받아오고 해제할 수 있도록 해준다.
    protected virtual void AddInputActionsCallbacks()
    {
        // walk 또는 Run 상태 진입 시의 키 입력을 받아오기 위한 이벤트 구독
        PlayerInput input = stateMachine.Player.Input;
        input.PlayerActions.Move.canceled += OnMoveCanceled;
        input.PlayerActions.Run.started += OnRunStarted;

        playerActions.Jump.started += OnJumpStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        // 위에서 Walk, Run 상태 진입 시 구독했던 이벤트 해제하기
        PlayerInput input = stateMachine.Player.Input;
        input.PlayerActions.Move.canceled -= OnMoveCanceled;
        input.PlayerActions.Run.started -= OnRunStarted;

        playerActions.Jump.started -= OnJumpStarted;
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnMoveCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }

    private void ReadMoveInput()
    {
        stateMachine.MoveInput = playerActions.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);
        Move(movementDirection);
    }

    // 카메라가 바라보고 있는 방향으로 움직이기
    private Vector3 GetMovementDirection()
    {
        // Main Camera의 정면, 우측 방향 가져오기
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        // 움직일 때 땅으로 꺼지지 않도록 y값 0으로 고정
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // 이동 방향 forward와 right에 입력된 벡터 값을 곱하여 이동 방향 구하기
        return forward * stateMachine.MoveInput.y + right * stateMachine.MoveInput.x;
    }

    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Rigidbody.velocity = movementDirection * movementSpeed;
    }

    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            // Quaternion.LookRotation(movementDirection) : movementDirection을 바라보는 Quaternion 생성
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            stateMachine.Player.transform.rotation = Quaternion.Slerp(stateMachine.Player.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MoveSpeed * stateMachine.MoveSpeedModifier;
        return movementSpeed;
    }

    // 애니메이션 처리
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }
}
