using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
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

            //If we are going to overshoot
            if (move.magnitude > distance)
            {
                //Move to target's exact location
                move = toTarget;
            }
            transform.position += move;
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4, 0, 4));
        }
    }
}
