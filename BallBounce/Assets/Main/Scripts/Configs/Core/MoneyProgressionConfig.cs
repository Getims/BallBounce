using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using Unity.VisualScripting.Dependencies.NCalc;

#endif

namespace Main.Scripts.Configs.Core
{
    [Serializable]
    public class MoneyProgressionConfig
    {
        [SerializeField]
        [InfoBox("Введите формулу для расчёта прогрессии, где LE - предыдущий элемент, а id - номер текущего элемента")]
        private string _progressionFormula = "LE * id";

        [SerializeField, Min(1)]
        private int _roundness = 1;

        [SerializeField, ListDrawerSettings(ShowIndexLabels = true, NumberOfItemsPerPage = 10)]
        [Min(0)]
        private List<int> _progression = new List<int>();

        public int Get(int id)
        {
            if (id < _progression.Count)
                return _progression[id];

            return _progression[^1];
        }

#if UNITY_EDITOR
        [Button]
        private void CalculateProgression()
        {
            int lastElement = 0;

            for (int i = 0; i < _progression.Count; i++)
            {
                _progression[i] = CalculateElement(lastElement, i, _progression[i]);
                lastElement = _progression[i];
            }
        }

        private int CalculateElement(int lastElement, int elementId, int currentValue)
        {
            var expression = new Expression(_progressionFormula);
            expression.Parameters["LE"] = lastElement;
            expression.Parameters["id"] = elementId;

            try
            {
                expression.Parameters["e"] = lastElement;

                int result = (int) expression.Evaluate(null);
                result = result / _roundness * _roundness;
                return result;
            }
            catch
            {
                Debug.LogError("Not correct formula");
                return currentValue;
            }
        }

#endif
    }
}