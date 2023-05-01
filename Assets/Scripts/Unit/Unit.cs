using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridNamespace;
using Actions;

//HOW TO MAKE NEW ACTIONS
//Step 1: Create New Action Script and Attach to Unit
//This is where the behaviour of the action takes place
//Step 2: Add new action variable here and grab in Awake
//Step 3: Create Getter for new action

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private GridPosition gridPosition;
        private MoveAction moveAction;
        private SpinAction spinAction;

        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
            spinAction = GetComponent<SpinAction>();
        }

        private void Start()
        {
            //Calculating unit's own grid position and setting it on grid
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        }

        private void Update()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
                LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
                gridPosition = newGridPosition;
            }
        }

        public MoveAction GetMoveAction()
        {
            return moveAction;
        }

        public SpinAction GetSpinAction() {
            return spinAction;
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }
    }
}
