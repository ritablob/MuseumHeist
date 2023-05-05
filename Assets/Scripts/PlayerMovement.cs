using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float invisibilityLimit = 10f;
    public float invisibilityTimer;
    private bool rechargingInvisibility;

    [SerializeField] List<MeshRenderer> renderers;
    List<Material> materials;
    [SerializeField] Material transparent;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastKnobRotation = PlayerPrefs.GetInt("LastRotation");
        Visible = true;
        lastRotation = 90;
        invisibilityTimer = 0;

        materials = new List<Material>();
        foreach (MeshRenderer rend in renderers) { materials.Add(rend.material); }
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
            if (!rechargingInvisibility)
            {
                SetVisibility(false);
                Debug.Log("invisible");
                StartCoroutine(InvisibilityTimer());
            }
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            if (!rechargingInvisibility)
            {
                SetVisibility(true);
                Debug.Log("visible");
            }
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
        if (!Visible)
        {
            EventManager.Instance.EventGo("AUDIO", "InvisibleOn");
            foreach (MeshRenderer rend in renderers) { rend.material = transparent; }
        }
        else
        {
            EventManager.Instance.EventGo("AUDIO", "InvisibleOff");
            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].material = materials[i];
            }
        }
    }

    public void Movement(bool shouldMove)
    {
        buttonPressed = shouldMove;
    }

    IEnumerator InvisibilityTimer()
    {
        if (invisibilityTimer < invisibilityLimit && !Visible)
        {
            yield return new WaitForSeconds(1);
            invisibilityTimer++;
            StartCoroutine(InvisibilityTimer());
        }
        else
        {
            Debug.Log("Timer has run out");
            if (!Visible)
                SetVisibility(true);
            rechargingInvisibility = true;
            StartCoroutine(Recharging());
        }
    }
    IEnumerator Recharging()
    {
        if (invisibilityTimer > 0)
        {
            rechargingInvisibility = true;
            yield return new WaitForSeconds(1);
            invisibilityTimer--;
            StartCoroutine(Recharging());
        }
        else
        {
            rechargingInvisibility = false;
            yield return null;
        }
    }
}
