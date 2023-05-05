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
    public bool Visible
    {
        get { return visible; }
        private set { visible = value; }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastKnobRotation = PlayerPrefs.GetInt("LastRotation");
        Visible = true;
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetVisibility(false);
            Debug.Log("invisible");
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            SetVisibility(true);
            Debug.Log("visible");
        }
        if (Input.GetKey(KeyCode.A)) Rotate(lastRotation - 1);
        if (Input.GetKey(KeyCode.D)) Rotate(lastRotation + 1);

        if (controller != null && buttonPressed && Visible) 
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

    public void SetVisibility(bool _visible) 
    {
        Visible = _visible;
        if (!Visible) EventManager.Instance.EventGo("AUDIO", "InvisibleOn");
        else EventManager.Instance.EventGo("AUDIO", "InvisibleOff");
    }

    public void Movement(bool shouldMove)
    {
        buttonPressed = shouldMove;
    }
}
