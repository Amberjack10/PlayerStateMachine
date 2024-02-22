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
        // ���콺 Ŀ�� �Ⱥ��̰� �ϱ�
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
        // ���� ��� �ִ��� ���θ� Ȯ���ϱ� ���� Raycast
        // ĳ������ �յ��¿쿡�� Ray�� �ٴ����� ���� �ٴڰ� �浹 ���θ� �����Ѵ�.
        // new Ray(transform.position + (transform.forward * 0.2f), Vector3.down) : �÷��̾��� ���⿡�� �ٴ��� ���� Raycast�� ���.
        // new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down) : ���� �ڵ��� ��� ���� �÷��̾�� �Ʒ����� Ray�� ������ ������
        // Raycast�� �߻� ������ ��¦ ���� �ø��� �ڵ�.
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            // rays �� �ϳ��� ���� ������� true
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) return true;
        }

        return false;
    }
}
