using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Units;

namespace GameUI
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private Button button;

        public void SetBaseAction(BaseAction baseAction) {
            textMeshPro.text = baseAction.GetActionName().ToUpper();

            button.onClick.AddListener(() => {
                //Creating an anonymous function aka a lambda expression
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
        }
    }
}
