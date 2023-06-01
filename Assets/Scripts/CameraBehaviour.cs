using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private List<Renderer> hiddenRenderers;
    [SerializeField] Transform player;

    RaycastHit hit;
    Ray ray;
    Color rayColour;
    Vector3 rayStartPosition;

    void Start()
    {
        hiddenRenderers = new List<Renderer>();
    }

    void Update()
    {
        if (!GameManagement.gameplayActive) return;
        rayStartPosition = transform.position;
        ray = new Ray(rayStartPosition, player.position - rayStartPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                AddToList(hit.collider.gameObject.GetComponent<Renderer>());
                rayColour = Color.green;
            }
            else
            {
                ClearList();
                rayColour = Color.red;
            }
            Debug.DrawRay(rayStartPosition, player.position - rayStartPosition, rayColour);
        }
    }

    private void AddToList(Renderer rend)
    {
        hiddenRenderers.Add(rend);
        rend.enabled = false;
    }

    private void ClearList()
    {
        foreach (Renderer rend in hiddenRenderers) 
        {
            rend.enabled = true;
        }
        hiddenRenderers.Clear();
    }
}
