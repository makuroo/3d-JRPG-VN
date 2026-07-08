using System;
using Battle;
using Character;
using Core;
using UnityEngine;

namespace UI.Battle
{
    public class BattleUIManager : MonoBehaviour
    {
        public static BattleUIManager Instance;
        public Action<BattleCharacterData> OnUnitSelected;
        [SerializeField] private ActionWindowUI _actionWindowUI;
        [SerializeField] private PartyStatusUI _partyStatusUI;
        [SerializeField] private BattleResultUI _battleResultUI;

        [SerializeField] private Texture2D _defultCursor;
        [SerializeField] private Texture2D _selectCursor;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            OnUnitSelected += UpdateSelectedUnit;
        }

        private void OnDisable()
        {
            OnUnitSelected -= UpdateSelectedUnit;
        
            if(BattleManager.Instance != null)
                BattleManager.Instance.OnStateChange -= ShowBattleResult;
        }

        private void Start()
        {
            //prevent battle manager null
            BattleManager.Instance.OnStateChange += ShowBattleResult;
        }

        private void UpdateSelectedUnit(BattleCharacterData selectedUnit)
        {
            if (selectedUnit == null)
            {
                Debug.LogWarning("Selected unit is null!");
                return;
			
            }

            if (selectedUnit.Team == Team.Player)
            {
                _actionWindowUI.PopulateWindow(selectedUnit.CharacterDataSo.Actions);
                _actionWindowUI.gameObject.SetActive(true);
            }
        }

        public void HideAllUI()
        {
            _actionWindowUI.gameObject.SetActive(false);
        }

        private void ShowBattleResult(BattleState state)
        {
            _battleResultUI.ShowResultUI(state);
        }

        public void ToggleCursor(bool select)
        {
            var cursor = select ? _selectCursor : _defultCursor;
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto); 
        }
    }
}