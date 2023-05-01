using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Units;

public class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;

    protected virtual void Awake() {
        unit = GetComponent<Unit>();
    }

}
