using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // PlayerGroundedState�� ��� �޴� ��� ��ũ��Ʈ������ GroundParameterHash�� true�� ���¿��� �����ϰ� �� ��!
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // TODO : Player�� ���� ����
        if (!stateMachine.Player.Controller.isGrounded && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }

    // Player�� ���°� Ground�� ��, �̵� Ű�� �Է��� ���߸� Idle ���·� �����ϰ� �����.
    protected override void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MoveInput == Vector2.zero) return;

        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnMoveCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void OnMove()
    {
        stateMachine.ChangeState(stateMachine.WalkState);
    }
}
