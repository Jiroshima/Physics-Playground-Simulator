using UnityEngine;

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

    // AudioSource for SFX
    private AudioSource walkingAudioSource;
    private AudioSource jumpAudioSource;

    // Public variables for sound effects
    public AudioClip walkingSound; // Footstep sound
    public AudioClip jumpSound;    // Jump sound
    private bool isWalking;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Create two separate AudioSources: one for walking, one for jumping
        walkingAudioSource = gameObject.AddComponent<AudioSource>();
        jumpAudioSource = gameObject.AddComponent<AudioSource>();

        // Set the walking audio source to loop for continuous walking sound
        walkingAudioSource.loop = true;
        walkingAudioSource.spatialBlend = 1f; // Optional: Set to 3D if needed
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset vertical velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input for horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction relative to player orientation
        Vector3 move = transform.right * x + transform.forward * z;

        // Check if player is moving (walking)
        if (move.magnitude > 0f && isGrounded)
        {
            if (!isWalking) // Play walking sound if the player starts walking
            {
                isWalking = true;
                walkingAudioSource.clip = walkingSound;
                walkingAudioSource.Play();
            }
        }
        else
        {
            if (isWalking) // Stop walking sound if the player stops
            {
                isWalking = false;
                walkingAudioSource.Stop();
            }
        }

        // Move the player horizontally
        controller.Move(move * speed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            // Play jump sound when jumping
            jumpAudioSource.PlayOneShot(jumpSound);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the player vertically
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }
}
