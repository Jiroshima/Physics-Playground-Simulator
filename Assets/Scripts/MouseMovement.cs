using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MouseMovement : MonoBehaviour

{
    public float mouseSensitivity = 300f;
    
    float xRotation = 0f;
    float yRotation = 0f; 

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        //removes cursor
         Cursor.lockState = CursorLockMode.Locked; 

    }
    
    void Update()
    {

    //getting mouse inputs 
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    // Rotate around the x axis (up and down)
    xRotation -= mouseY;
    
    // Clamp the rotation
    xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

    // Rotation around the y-axis (left and right)
    yRotation += mouseX;

    // Apply rotations to our transform
    transform.localRotation = Quaternion.Euler (xRotation, yRotation, 0f);

    }
}

    

