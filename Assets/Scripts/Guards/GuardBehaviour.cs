using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour
{
    [SerializeField] private LEDBlinking led;

    RaycastHit hit;
    Ray ray;
    Color rayColour;
    Vector3 rayStartPosition;
    bool playerVisible;

    public List<Transform> coordinateList = new();
    public float marginOfError = 0.9f;

    int currentCoordinate;
    public NavMeshAgent navMeshAgent;

    public Transform player;
    Vector3 lastKnownPlayerPosition;

    void Start()
    {
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!GameManagement.guardsActive) return;
        /* To-do:
         * - check if player visible (+ margin of delay time) with raycasting
         * - - if not, proceed to go towards one of the points (with navmesh)
         * - - if yes, follow player until it is not visible anymore, and then walk in that direction some time more,
         * and then continue to the usual route
         */
        rayStartPosition = transform.position;
        rayStartPosition.y += 2f;
        if (!playerVisible)
        {
            if (navMeshAgent.remainingDistance < 1f)
            {
                currentCoordinate++;
                if (currentCoordinate >= coordinateList.Count)
                {
                    currentCoordinate = 0;
                }
            }
            navMeshAgent.SetDestination(coordinateList[currentCoordinate].position);
        }
        else
        {
            /*if (navMeshAgent.remainingDistance < 1f)
            {
            // game over
                isLookingForPlayer = false;
            }*/

            navMeshAgent.SetDestination(lastKnownPlayerPosition);
        }

        ray = new Ray(rayStartPosition, player.position - rayStartPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!playerVisible) led.GuardSeesPlayer(true);
                playerVisible = true;
                rayColour = Color.green;
                lastKnownPlayerPosition = player.position;
            }
            else
            {
                if (playerVisible) led.GuardSeesPlayer(false);
                playerVisible = false;
                rayColour = Color.red;
            }
            Debug.DrawRay(rayStartPosition, player.position - rayStartPosition, rayColour);
        }
    }
}
