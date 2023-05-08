using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GuardBehaviour : MonoBehaviour
{
    [SerializeField] private LEDBlinking led;
    private Buzzer buzzer;
    private AudioSource audioSource;

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

    public WinManager winManager;
    PlayerMovement playerM;

    void Start()
    {
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
        buzzer = GetComponent<Buzzer>();
        audioSource = GetComponent<AudioSource>();
        playerM = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!GameManagement.guardsActive)
        {
            navMeshAgent.isStopped = true;
            led.GuardSeesPlayer(false);
            buzzer.ActivateBuzzer(false);
        }
        else
        {
            navMeshAgent.isStopped = false;
        }

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
            if (!playerM.Visible)
            {
                playerVisible = false;
                led.GuardSeesPlayer(false);
                buzzer.ActivateBuzzer(false);
                audioSource.Stop();

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
                navMeshAgent.SetDestination(lastKnownPlayerPosition);
            }
        }

        ray = new Ray(rayStartPosition, player.position - rayStartPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!playerVisible && playerM.Visible && GameManagement.guardsActive)
                {
                    led.GuardSeesPlayer(true);
                    buzzer.ActivateBuzzer(true);
                    audioSource.Play();
                }
                playerVisible = true;
                rayColour = Color.green;
                lastKnownPlayerPosition = player.position;
            }
            else
            {
                if (playerVisible)
                {
                    led.GuardSeesPlayer(false);
                    buzzer.ActivateBuzzer(false);
                    audioSource.Stop();
                }
                playerVisible = false;
                rayColour = Color.red;
            }
            Debug.DrawRay(rayStartPosition, player.position - rayStartPosition, rayColour);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>() != null && !collision.gameObject.GetComponent<PlayerMovement>().Visible) return;

            navMeshAgent.isStopped = true;
            winManager.CheckLose();
        }
    }

    public void StartPuzzle()
    {
        audioSource.Stop();
        playerVisible = false;
        buzzer.ActivateBuzzer(false);
        led.GuardSeesPlayer(false);
    }
}
