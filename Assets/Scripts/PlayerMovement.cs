using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    bool buttonPressed = false;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed;

    private int lastRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastRotation = PlayerPrefs.GetInt("lastRotation", lastRotation);
    }

    void Update()
    {
        // remove once arduino is set up
        if (Input.GetKeyDown(KeyCode.W)) buttonPressed = true;
        if (Input.GetKeyUp(KeyCode.W)) buttonPressed = false;

        if (controller != null && buttonPressed) 
        {
            controller.Move(transform.forward * Time.deltaTime * movementSpeed);
        }
    }

    public void Rotate(int value)
    {
        // rotate based on difference between value and last value
        float difference = value - lastRotation;
        float rotationAmount = (value - lastRotation) * rotationSpeed;
        if (difference > 150) rotationAmount = 0;
        transform.Rotate(Vector3.up, rotationAmount);

        // save value for next time
        lastRotation = value;
        PlayerPrefs.SetInt("lastRotation", lastRotation);
    }
}
