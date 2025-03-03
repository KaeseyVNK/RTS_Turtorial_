using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;

    public bool isCommandedToMove;

    DirectionIndicator directionIndicator;

    public void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();

        directionIndicator = GetComponent<DirectionIndicator>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1) && IsMovingPossible()  )
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                isCommandedToMove = true;
                StartCoroutine(NoCommanded());

                agent.SetDestination(hit.point);

                // play the unit command sound
                SoundManager.Instance.PlayUnitCommandSound();

                directionIndicator.DrawLine(hit);
            }
        }

        //agent reached destination
        // if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        // {
        //     isCommandedToMove = false;
        // }
    }


    IEnumerator NoCommanded()
    {
        yield return new WaitForSeconds(1f);
        isCommandedToMove = false;
    }

    private bool IsMovingPossible()
    {
        return CursorManager.Instance.currentCursor != CursorManager.CursorType.UnAvailable;
    }
}
