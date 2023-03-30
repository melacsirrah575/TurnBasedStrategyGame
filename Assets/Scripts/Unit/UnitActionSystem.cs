using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionSystem
{
    //THIS SCRIPT IS A SINGLETON
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }

        public event EventHandler OnSelectedUnitChanged;

        [SerializeField] private Unit selectedUnit;
        [SerializeField] private LayerMask unitLayerMask;

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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (TryHandleUnitSelection()) return;

                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)) 
                {
                    selectedUnit.GetMoveAction().Move(mouseGridPosition);
                }
            }
        }

        private bool TryHandleUnitSelection()
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

            return false;
        }

        private void SetSelectedUnit(Unit unit)
        {
            selectedUnit = unit;
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
    }
}
