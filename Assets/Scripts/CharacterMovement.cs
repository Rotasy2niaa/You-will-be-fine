using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;

    [SerializeField] private Transform targetCamTransform; // 摄像机 transform（Main Camera）
    [Header("Mouse Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    private float xRotation = 0f;

    [Header("Move Speed")]
    [SerializeField] private float horizontalSpeed;
    [Header("Jump Force")]
    [SerializeField] private float jumpSpeed;
    [Header("Gravity Settings")]
    [SerializeField] private float gravity;
    [SerializeField] private float extraGravity;
    [SerializeField] private float coyoteTime;
    [Header("Ground Check Settings")]
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private float groundCheckR;
    [SerializeField] private LayerMask whatIsGround;

    private CharacterController characterController;

    private float verticalSpeed;
    private float jumpKeyDownT;
    private bool grounded;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        characterController = GetComponent<CharacterController>();

        verticalSpeed = 0f;
        jumpKeyDownT = -coyoteTime - 1f;
        grounded = false;

        Cursor.lockState = CursorLockMode.Locked; // 鼠标锁定在屏幕中央
        Cursor.visible = false; // 可选：隐藏鼠标光标
    }

    void Update()
    {
        HandleMouseLook();

        if (Input.GetKeyDown(KeyCode.Space)) jumpKeyDownT = Time.time;
    }

    private void FixedUpdate()
    {
        // 获取移动输入
        Vector3 movementVec =
            (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized;
        characterController.Move(movementVec * horizontalSpeed * Time.fixedDeltaTime);

        // 地面检测
        grounded = Physics.CheckSphere(transform.position + groundCheckOffset, groundCheckR, whatIsGround);
        if (grounded)
        {
            if (Time.time < jumpKeyDownT + coyoteTime) verticalSpeed = jumpSpeed;
        }
        else
        {
            verticalSpeed += gravity * Time.fixedDeltaTime;
            if (!Input.GetKey(KeyCode.Space)) verticalSpeed += extraGravity * Time.fixedDeltaTime;
        }

        characterController.Move(Vector3.up * verticalSpeed * Time.fixedDeltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 角色左右旋转（Yaw）
        transform.Rotate(Vector3.up * mouseX);

        // 相机上下旋转（Pitch）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        targetCamTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckR);
    }
}
