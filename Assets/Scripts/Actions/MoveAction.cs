using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using GridNamespace;
using Units;

namespace Actions
{
    public class MoveAction : BaseAction
    {
        [SerializeField] Animator unitAnimator;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private int maxMoveDistance = 4;


        private Vector3 targetPosition;

        protected override void Awake()
        {
            base.Awake();
            targetPosition = transform.position;
        }

        void Update()
        {
            if (!isActive) return;

            float stoppingDistance = 0.1f;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

                unitAnimator.SetBool("IsWalking", true);
            }
            else
            {
                unitAnimator.SetBool("IsWalking", false);
                isActive = false;
                onActionComplete();
            }

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }

        public void Move(GridPosition gridPosition, Action onActionComplete)
        {
            this.onActionComplete = onActionComplete;
            this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            isActive = true;
        }

        public bool IsValidActionGridPosition(GridPosition gridPosition)
        {
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

        public override string GetActionName()
        {
            return "Move";
        }
    }
}
