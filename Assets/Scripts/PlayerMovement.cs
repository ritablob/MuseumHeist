using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    bool buttonPressed = false;

    [SerializeField] private float movementSpeed = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) buttonPressed = true;
        if (Input.GetKeyUp(KeyCode.W)) buttonPressed = false;

        if (controller != null && buttonPressed) 
        {
            controller.Move(transform.forward * Time.deltaTime * movementSpeed);
        }
    }
}
