using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f;     // 저항

    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        // 현재 플레이어가 땅에 위치해 있으면서 상태가 Ground라면
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            // Physics.gravity.y : 중력 가속도 -9.7을 의미. 따라서 verticalVelocity는 위로 솟아날 경우를 제외하고 항상 음수일 것이다. 
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // 현재 플레이어가 공중에 있으면 Physics.gravity.y * Time.deltaTime 만큼 가속.
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        // impact가 Vector3.zero가 될 때까지 감소된다.
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void Reset()
    {
        // 플레이어가 땅에 닿으면 impact, verticalVelocity 초기화
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
