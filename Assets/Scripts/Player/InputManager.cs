using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerIput;
    public PlayerInput.OnFootActions onFootActions;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;

    void Awake()
    {
        playerIput = new PlayerInput();
        onFootActions = playerIput.OnFoot;

        playerMotor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();

        //onFootActions.Jump.performed += ctx => playerMotor.Jump();
    }

    void FixedUpdate()
    {
        //// tell the playermotor to move using the value from our movement action
        //playerMotor.ProcessMove(onFootActions.Movement.ReadValue<Vector2>());

        //// check event for sprinting if holding shift
        //if (onFootActions.Sprint.ReadValue<float>() > 0)
        //{
        //    playerMotor.Sprint();
        //}
        //else
        //{
        //    playerMotor.Walk();
        //}
    }

    private void LateUpdate()
    {
        //playerLook.ProcessLook(onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFootActions.Enable();
    }

    private void OnDisable()
    {
        onFootActions.Disable();
    }
}
