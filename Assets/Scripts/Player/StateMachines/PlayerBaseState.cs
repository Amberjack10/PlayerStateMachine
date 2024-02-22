using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    // ��� State�� StateMachine�� �������� �� ��!
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

    // InputAction�� �̺�Ʈ�� �����ϴ� �Ͱ� �����ϰ� Callback�� �ɾ��ش�.
    // �̸� Enter�� Exit�� �ɾ�ξ� State ���� �� InputAction�� �޾ƿ��� ������ �� �ֵ��� ���ش�.
    protected virtual void AddInputActionsCallbacks()
    {
        // walk �Ǵ� Run ���� ���� ���� Ű �Է��� �޾ƿ��� ���� �̺�Ʈ ����
        PlayerInput input = stateMachine.Player.Input;
        input.PlayerActions.Move.canceled += OnMoveCanceled;
        input.PlayerActions.Run.started += OnRunStarted;

        playerActions.Jump.started += OnJumpStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        // ������ Walk, Run ���� ���� �� �����ߴ� �̺�Ʈ �����ϱ�
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

    // ī�޶� �ٶ󺸰� �ִ� �������� �����̱�
    private Vector3 GetMovementDirection()
    {
        // Main Camera�� ����, ���� ���� ��������
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        // ������ �� ������ ������ �ʵ��� y�� 0���� ����
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // �̵� ���� forward�� right�� �Էµ� ���� ���� ���Ͽ� �̵� ���� ���ϱ�
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
            // Quaternion.LookRotation(movementDirection) : movementDirection�� �ٶ󺸴� Quaternion ����
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            stateMachine.Player.transform.rotation = Quaternion.Slerp(stateMachine.Player.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MoveSpeed * stateMachine.MoveSpeedModifier;
        return movementSpeed;
    }

    // �ִϸ��̼� ó��
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }
}
