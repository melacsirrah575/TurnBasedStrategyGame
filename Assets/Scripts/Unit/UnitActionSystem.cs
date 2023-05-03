using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public event EventHandler OnSelectedActionChanged;

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
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (TryHandleUnitSelection()) return;

            HandleSelectedAction();
        }

        private void HandleSelectedAction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);

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
                    if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                    {
                        if (unit == selectedUnit)
                        {
                            //Unitis already selected
                            return false;
                        }

                        SetSelectedUnit(unit);
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

            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        }

        public BaseAction GetSelectedAction()
        {
            return selectedAction;
        }
    }
}
