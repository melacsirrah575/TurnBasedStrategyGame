using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;
using GridNamespace;
using Actions;

namespace Units
{
    //THIS SCRIPT IS A SINGLETON
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }

        public event EventHandler OnSelectedUnitChanged;

        [SerializeField] private Unit selectedUnit;
        [SerializeField] private LayerMask unitLayerMask;

        private BaseAction selectedAction;
        private bool isBusy;

        //Turning this script into a Singleton
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one UnitActionSystem {transform} - {Instance}");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            SetSelectedUnit(selectedUnit);
        }

        private void Update()
        {
            if (isBusy) return;
            if (TryHandleUnitSelection()) return;

            HandleSelectedAction();
        }

        private void HandleSelectedAction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                switch (selectedAction)
                {
                    case MoveAction moveAction:

                        if (moveAction.IsValidActionGridPosition(mouseGridPosition))
                        {
                            SetBusy();
                            moveAction.Move(mouseGridPosition, ClearBusy);
                        }
                        break;
                    case SpinAction spinAction:

                        SetBusy();
                        //Calling CleaBusy by way of a delegate in SpinAction script
                        spinAction.Spin(ClearBusy);
                        break;
                }
            }
        }

        private void SetBusy()
        {
            isBusy = true;
        }

        private void ClearBusy()
        {
            isBusy = false;
        }

        private bool TryHandleUnitSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, unitLayerMask))
                {
                    if (hit.transform.TryGetComponent<Unit>(out Unit selectedUnit))
                    {
                        SetSelectedUnit(selectedUnit);
                        return true;
                    }
                }
            }
            return false;

        }

        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;

            //Defaulting the initial selected action to the Move action
            SetSelectedAction(unit.GetMoveAction());
            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

            //COMMENTED CODE BELOW EXPLAINS WHAT ABOVE ONSELECTUNITCHANGED IS DOING
            //if (OnSelectedUnitChanged != null)
            //{
            //    //'this' is a reference to the object that fired off the event
            //    OnSelectedUnitChanged(this, EventArgs.Empty);
            //}
        }

        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            selectedAction = baseAction;
        }
    }
}
