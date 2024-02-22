using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MoveSpeedModifier = 0f;    // Idle 상태 진입 시 움직이지 않도록 속도를 0으로 변경해준다.
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.MoveInput != Vector2.zero)
        {
            // 이동 처리가 들어왔다면 플레이어의 상태를 Idle에서 Walk로 변경해주기
            OnMove();
            return;
        }
    }
}
