using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector3;
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity; 

    bool isGrounded;

    private Vector3 lastPosition = new Vector3(0f,0f,0f);
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Checks if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Reset the default velocity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // get inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create the moving vector
        Vector3 move = transform.right * x + transform.forward * z; // right is the x axis forward is the z axis

        // Move the player
        controller.Move(move * speed * Time.deltaTime);

        // check if player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Actually jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Falling Down
        velocity.y += gravity * Time.deltaTime;

        // Execute the jump
        controller.Move(velocity * Time.deltaTime);

    }
}

