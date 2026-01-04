using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GroundController : MonoBehaviour
{
    [Header("移動與旋轉速度")]
    public float moveSpeed = 10f;
    public float rotateSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 確保勾選 Kinematic，物理旋轉才會產生摩擦力帶動物體
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        HandleKeyboardMovement();
        HandleKeyboardRotation();
    }

    void HandleKeyboardMovement()
    {
        // WASD 控制前後左右 (使用世界座標或自身座標)
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        // 計算移動向量 (以世界空間為準，或是 transform.forward 以盤子面為準)
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude > 0)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void HandleKeyboardRotation()
    {
        // QE 控制順時針與逆時針旋轉 (繞著 Y 軸)
        float rotationInput = 0f;

        if (Input.GetKey(KeyCode.E)) rotationInput = 1f;  // 順時針
        if (Input.GetKey(KeyCode.Q)) rotationInput = -1f; // 逆時針

        if (rotationInput != 0)
        {
            // 計算旋轉量
            Quaternion deltaRotation = Quaternion.Euler(0, rotationInput * rotateSpeed * Time.fixedDeltaTime, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}