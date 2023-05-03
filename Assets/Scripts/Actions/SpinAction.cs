using System;
using System.Collections;
using System.Collections.Generic;
using GridNamespace;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
        private float totalSpinAmount;

        private void Update()
        {
            if (!isActive) return;

            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

            totalSpinAmount += spinAddAmount;
            if (totalSpinAmount >= 360)
            {
                isActive = false;
                //This is where the delegate will callback the function we are passing
                onActionComplete();
            }

        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            this.onActionComplete = onActionComplete;
            isActive = true;
            totalSpinAmount = 0;
            Debug.Log("Spin Action Called");
        }

        public override string GetActionName()
        {
            return "Spin";
        }

                public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            GridPosition unitGridPosition = unit.GetGridPosition();

            return new List<GridPosition> {
                unitGridPosition
            };
        }
    }
}
