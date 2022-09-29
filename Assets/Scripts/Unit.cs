using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionSystem;

public class Unit : MonoBehaviour
{
    [SerializeField] Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;

    private Vector3 targetPosition;

    #region Public Methods
    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    #endregion

    #region Private Methods
    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        Vector3 toTarget = targetPosition - transform.position;
        float distance = toTarget.magnitude;

        if (distance > 0)
        {
            Vector3 move = toTarget.normalized * moveSpeed * Time.deltaTime;

            //This rotate is not a linear rotation because our transform.forward is constantly being updated.
            //To make it linear, one would need to store starting forward value and only have time change
            transform.forward = Vector3.Lerp(transform.forward, toTarget, Time.deltaTime * rotateSpeed);

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
    }

    #endregion
}
