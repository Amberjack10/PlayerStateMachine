using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // PlayerGroundedState를 상속 받는 모든 스크립트에서는 GroundParameterHash가 true인 상태에서 동작하게 될 것!
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

        // TODO : Player의 지면 감지
    }

    // Player의 상태가 Ground일 때, 이동 키의 입력을 멈추면 Idle 상태로 진입하게 만든다.
    // Air 상태일 때나 Attack 상태일 때는 이동 키의 입력을 멈췄다고 Idle로 변경되면 안되기 때문에 Ground에서 처리해준다.
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
