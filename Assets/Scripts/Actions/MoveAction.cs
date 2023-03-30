using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int maxMoveDistance = 4;


    private Vector3 targetPosition;
    private Unit unit;

    private void Awake()
    {
        targetPosition = transform.position;
        unit = GetComponent<Unit>();
    }

    void Update()
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

    public void Move(GridPosition gridPosition)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition) {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPositon = unitGridPosition + offsetGridPosition;

                //Invalid location test
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPositon)) continue;
                //Position is the same as the current GridPosition test
                if (unitGridPosition == testGridPositon) continue;
                //Position has another unit on it test
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPositon)) continue;

                validGridPositionList.Add(testGridPositon);
            }
        }

        return validGridPositionList;
    }
}
