using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5.0f;
    public float gravity = -9.8f;
    public float jumpHeight = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
    }

    /**
     * Di chuyen player
     * input la 1 vector di chuyen theo truc x, y
     */
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        // Normalize the move direction to prevent faster diagonal movement
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    /*
     * Thuc hien jump cho Player
     */
    public void Jump()
    {
        if (isGrounded)
        {
            // Cong thuc tinh van toc 
            // v^2 = u^2 + 2 * a * s
            // u = sqrt(-2 * a * s)
            playerVelocity.y = (float)Math.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    public void Sprint()
    {
        speed = 10.0f;
    }

    public void Walk()
    {
        speed = 5.0f;
    }
}
