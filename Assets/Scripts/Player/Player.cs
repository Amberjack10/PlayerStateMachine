using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public LayerMask groundLayerMask;

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInput Input { get; private set; }

    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInput>();

        stateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        // 마우스 커서 안보이게 하기
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public bool IsGrounded()
    {
        // 땅을 밟고 있는지 여부를 확인하기 위한 Raycast
        // 캐릭터의 앞뒤좌우에서 Ray를 바닥으로 쏴서 바닥과 충돌 여부를 감지한다.
        // new Ray(transform.position + (transform.forward * 0.2f), Vector3.down) : 플레이어의 방향에서 바닥을 향해 Raycast를 쏜다.
        // new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down) : 위의 코드의 경우 실제 플레이어보다 아래에서 Ray가 나가기 때문에
        // Raycast의 발사 지점을 살짝 위로 올리는 코드.
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            // rays 중 하나라도 땅에 닿았으면 true
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) return true;
        }

        return false;
    }
}
