using System;
using Data.UnitDatabase;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.MainMenu
{
    public class StartingUnitUI : MonoBehaviour
    {
        [SerializeField] private StartingUnitButton _buttonPrefab;
        [SerializeField] private Transform _buttonParent;
        [SerializeField] private UnitDatabaseSO _unitDatabase;

        private void Awake()
        {
            foreach (var unit in _unitDatabase.Database)
            {
                var button = Instantiate(_buttonPrefab, _buttonParent);
                button.Setup(unit);
            }
        }
    }
}
