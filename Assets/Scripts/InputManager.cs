using UnityEngine;
using UnityEngine.InputSystem;
// imports unity engine's input system

public class InputManager : MonoBehaviour 
{
     // reference to player input and onfootaction map
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    


    // creates new instance of playerInput class 
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        // anytime jump is performed, use ctx to call motor.jump function
        onFoot.Jump.performed += ctx => motor.Jump();
        look = GetComponent<PlayerLook>();
    }

    void FixedUpdate()
    {
        //tell the playermotor to move using value from movement action 
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    // enables action map


    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());

    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    
    // disable action map
    private void OnDisable()
    {
        onFoot.Disable();
    }
    
}

    



