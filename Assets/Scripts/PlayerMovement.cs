using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    bool buttonPressed = false;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed;

    public int lastRotation;
    public int lastKnobRotation;
    private bool visible;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastKnobRotation = PlayerPrefs.GetInt("LastRotation");
        visible = true;
        lastRotation = 90;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("LastRotation", lastKnobRotation);
    }

    void Update()
    {
        if (!GameManagement.guardsActive) return;

        // remove once arduino is set up
        if (Input.GetKeyDown(KeyCode.W)) buttonPressed = true;
        if (Input.GetKeyUp(KeyCode.W)) buttonPressed = false;
        if (Input.GetKeyDown(KeyCode.I)) Invisibility(visible);
        if (Input.GetKey(KeyCode.A)) Rotate(lastRotation - 1);
        if (Input.GetKey(KeyCode.D)) Rotate(lastRotation + 1);

        if (controller != null && buttonPressed && visible) 
        {
            controller.Move(transform.forward * Time.deltaTime * movementSpeed);
        }
    }

    public void Rotate(int value)
    {
        if (!GameManagement.guardsActive) return;

        // rotate based on difference between value and last value
        float difference = value - lastRotation + lastKnobRotation;
        float rotationAmount = (value - lastRotation) * rotationSpeed;
        if (difference > 150) rotationAmount = 0;
        transform.Rotate(Vector3.up, rotationAmount);

        // save value for next time
        lastRotation = value;
    }

    public void Invisibility(bool invisible)
    {
        visible = !invisible;
    }

    public void Movement(bool shouldMove)
    {
        buttonPressed = shouldMove;
    }
}
