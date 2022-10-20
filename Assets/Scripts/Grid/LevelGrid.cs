using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT IS A SINGLETON
public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one LevelGrid {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    //Gets GridObject from system and sets a unit at a specific position
    //GridObject stores the unit that is passed through
    public void AddUnitAtGridPosition(GridPosition position, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition position)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition position, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }

    //This is no different than writing a return function
    //Often used when just wanting to pass something through a system
    public GridPosition GetGridPosition(Vector3 worldPostion) => gridSystem.GetGridPosition(worldPostion);
}
