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
    private GridPosition gridPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        //Calculating unit's own grid position and setting it on grid
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
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

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

}
