using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector3;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    // boolean to tell if player is grounded
    private bool isGrounded; 
    // public float for speed with default value 5
    public float speed = 5f;
    // public float for gravity with default value -9.8
    public float gravity = -9.8f;
    // public float for jumpheight with default value 3 
    public float jumpHeight = 1.5f;
    

    void Start()
    {
        controller = GetComponent<CharacterController>(); 

    }


    void Update()
    {
        //returns value every single frame
        isGrounded = controller.isGrounded;
    }
    //receives the input for InputManager.cs then apply them to character controller
    //translates vertical movement to forward backward movement
    public void ProcessMove(Vector2 input)
    {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
            //apply constant downward force to player
            playerVelocity.y += gravity * Time.deltaTime; 
            //check if value of ifGrounded and player is less than 0, then sets it to -2 if it is
            if(isGrounded && playerVelocity.y < 0)
                playerVelocity.y = -2f;
            controller.Move(playerVelocity * Time.deltaTime);
            Debug.Log(playerVelocity.y);
    }

    public void Jump()
    {
        // ensures that player is able to jump when they are grounded.
        // square root makes velocity proportional to desired jump height 
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
