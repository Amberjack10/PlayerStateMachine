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

        // TODO : Grounded로 상태 전환
        if (stateMachine.Player.IsGrounded())
        {
            // 플레이어가 땅에 닿으면 Idle 상태로 전환
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
