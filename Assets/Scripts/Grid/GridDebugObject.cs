using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GridNamespace
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMeshPro;
        private GridObject gridObject;
        public void SetGridObject(GridObject gridObject)
        {
            this.gridObject = gridObject;
        }

        private void Update()
        {
            textMeshPro.text = gridObject.ToString();
        }
    }
}

