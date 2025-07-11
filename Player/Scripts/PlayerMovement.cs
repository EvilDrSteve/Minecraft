using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Camera playerCamera;
    public Transform groundCheck;
    public Vector3 groundCheckSize;
    public LayerMask groundMask;
    public float speed = 12f;
    public float sprintSpeed = 25f;
    public float jumpHeight = 1f;
    public float gravity = -9.81f;
    public float sprintFov = 60f;
    public float fovZoomSpeed = 1f;
    float fov = 70f;

    Vector3 velocity;
    bool isGrounded;
    void Update()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, groundCheckSize / 2, Quaternion.identity, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        Vector3 move = transform.right * x + transform.forward * z;
        move *= sprint ? sprintSpeed : speed;

        if (sprint)
        {
            playerCamera.fieldOfView = Vector2.MoveTowards(new Vector2(playerCamera.fieldOfView, 0), new Vector2(sprintFov, 0), fovZoomSpeed * Time.deltaTime).x;
        } else playerCamera.fieldOfView = Vector2.MoveTowards(new Vector2(playerCamera.fieldOfView, 0), new Vector2(fov, 0), fovZoomSpeed * Time.deltaTime).x;
        controller.Move(move * Time.deltaTime);
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
