using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

public class Unit : MonoBehaviour
{
    [SerializeField] Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4f;

    private Vector3 targetPosition;

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 toTarget = targetPosition - transform.position;
        float distance = toTarget.magnitude;

        if (distance > 0)
        {
            Vector3 move = toTarget.normalized * moveSpeed * Time.deltaTime;
            unitAnimator.SetBool("IsWalking", true);

            //If we are going to overshoot
            if (move.magnitude > distance)
            {
                //Move to target's exact location
                move = toTarget;
            }
            transform.position += move;
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }


        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }
}
