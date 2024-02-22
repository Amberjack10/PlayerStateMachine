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
        stateMachine.MoveSpeedModifier = 0f;    // Idle ���� ���� �� �������� �ʵ��� �ӵ��� 0���� �������ش�.
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
            // �̵� ó���� ���Դٸ� �÷��̾��� ���¸� Idle���� Walk�� �������ֱ�
            OnMove();
            return;
        }
    }
}
