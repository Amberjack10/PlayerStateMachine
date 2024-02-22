public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // TODO : Grounded�� ���� ��ȯ
        if (stateMachine.Player.IsGrounded())
        {
            // �÷��̾ ���� ������ Idle ���·� ��ȯ
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
