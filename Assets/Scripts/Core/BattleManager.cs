using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Character;
using Character.Enemy;
using Data.ActionData;
using Data.CharacterData;
using Sirenix.OdinInspector;
using UI.Battle;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Core
{
    public enum BattleState
    {
        BattleStart,
        PlayerTurn,
        EnemyTurn,
        BattleWon,
        BattleLost
    }

    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;
        public Action<BattleState> OnStateChange;
        public Action<CharacterDataSo, CharacterDataSo> OnPrepareBattle;
        public Action<List<BattleCharacterData>> OnPartyInitialized;
        [ShowInInspector,ReadOnly]
        private BattleState _currentState = BattleState.BattleStart;
        public BattleState CurrentState => _currentState;
        public BattleCharacterData ActiveUnit { get; set; }

        public BattleActionSO SelectedBattleAction => _selectedBattleAction;
    
        private List<BattleCharacterData> _playerPartyData = new();
        private List<BattleCharacterData> _enemyPartyData = new();
    
        private BattleActionSO _selectedBattleAction;
        private int _currentPlayerIndex;
        private int _currentEnemyIndex;

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
            DontDestroyOnLoad(gameObject);
        }

        public void SetBattle(CharacterDataSo playerData, CharacterDataSo enemyData)
        {
            ScreenFader.Instance.FadeIn(.5f, () =>
            {
                StartCoroutine(TransitionCoroutine(playerData, enemyData));
            });
        }
    
        private IEnumerator TransitionCoroutine(CharacterDataSo playerData, CharacterDataSo enemyData)
        {
            var handler = SceneManager.LoadSceneAsync("BattleScene");
            yield return handler;
            GameManager.Instance.UpdateState(GameState.Battle);
            AudioManager.Instance.Crossfade("Battle",.5f);
            OnPrepareBattle?.Invoke(playerData,enemyData);
            ScreenFader.Instance.FadeOut(.5f);
        }

        public void InitializeParty(List<BattleCharacterData> playerParty, List<BattleCharacterData> enemyParty)
        {
            _playerPartyData = playerParty;
            _enemyPartyData = enemyParty;
        
            OnPartyInitialized?.Invoke(playerParty);
            OnStateChange?.Invoke(BattleState.PlayerTurn);
            _currentState = BattleState.PlayerTurn;
        
            StateUpdate(_currentState);
        }

        private void SetSelectedUnit(BattleCharacterData unit)
        {
            if (ActiveUnit != null)
            {
                ActiveUnit.BattleCharacterView.ShowHighlight(false);
            }
        
            ActiveUnit = unit;
            foreach (var partyUnit in _playerPartyData)
            {
                if (partyUnit != null)
                {
                    partyUnit.BattleCharacterView.ShowHighlight(ActiveUnit == partyUnit);
                }
            }
            BattleUIManager.Instance.OnUnitSelected?.Invoke(ActiveUnit);
        }
    
        public void OnActionSelected(BattleActionSO action)
        {
            _selectedBattleAction = action;
        
            if(ActiveUnit.Team != Team.Player) return;
            BattleUIManager.Instance.ToggleCursor(true);
            foreach (var enemy in _enemyPartyData)
            {
                enemy.BattleCharacterView.SetSelectable(true);
            }
        }

        public void ExecuteActionCommand(BattleCharacterData target)
        {
            BattleUIManager.Instance.ToggleCursor(false);
            foreach (var partyUnit in _enemyPartyData)
            {
                partyUnit.BattleCharacterView.SetSelectable(false);
                partyUnit.BattleCharacterView.ShowHighlight(false);
            }
        
            var command = SelectedBattleAction.CreateBattleCommand(ActiveUnit, target);
            StartCoroutine(ExecuteCommandCoroutine(command));
        }

        private IEnumerator ExecuteCommandCoroutine(BattleCommand command)
        {
            BattleUIManager.Instance.HideAllUI();
            foreach (var partyUnit in _enemyPartyData)
            {
                partyUnit.BattleCharacterView.SetSelectable(false);
            }
            foreach (var partyUnit in _playerPartyData)
            {
                partyUnit.BattleCharacterView.SetSelectable(false);
            }
            yield return StartCoroutine(command.Execute());
            _selectedBattleAction = null;
            DetermineOutcome();
        }

        private bool IsPartyDefeated(List<BattleCharacterData> party)
        {
            foreach (var data in party)
            {
                if (data.CharacterStat.Stat.CurrentHealth > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void DetermineOutcome()
        {
            if (IsPartyDefeated(_playerPartyData))
            {
            
                OnStateChange?.Invoke(BattleState.BattleLost);
                _currentState = BattleState.BattleLost;
                return;
            }

            if (IsPartyDefeated(_enemyPartyData))
            {
                OnStateChange?.Invoke(BattleState.BattleWon);
                _currentState = BattleState.BattleWon;
                return;
            }

            var nextState = _currentState == BattleState.PlayerTurn ? BattleState.EnemyTurn : BattleState.PlayerTurn;
          
            OnStateChange?.Invoke(nextState);
            _currentState = nextState;
        
            StateUpdate(_currentState);
        }

        private void StateUpdate(BattleState nextState)
        {
            switch (nextState)
            {
                case BattleState.PlayerTurn:
                    PlayerTurn();
                    break;
                case BattleState.EnemyTurn:
                    EnemyTurn();
                    break;
            }
        }

        private void PlayerTurn()
        {
            var unit = GetAvailableUnit(_playerPartyData, ref _currentPlayerIndex);
            SetSelectedUnit(unit);
        }

        private void EnemyTurn()
        {
            var unit = GetAvailableUnit(_enemyPartyData, ref _currentEnemyIndex);
            SetSelectedUnit(unit);
            ActiveUnit.CharacterStat.GetComponent<EnemyAI>().PickCombatMove();
        }

        public BattleCharacterData GetRandomPlayerUnit()
        {
            return _playerPartyData[Random.Range(0, _playerPartyData.Count)];
        }

        public void ReturnToOverworld()
        {
            ScreenFader.Instance.FadeIn(.5f, () =>
            {
                ScreenFader.Instance.StartCoroutine(ScreenTransition());
            });
        }
    
        private IEnumerator ScreenTransition()
        {
            var handler = SceneManager.LoadSceneAsync("Overworld");
            yield return handler;
            ScreenFader.Instance.FadeOut(.5f);
            GameManager.Instance.UpdateState(GameState.Exploration);
            AudioManager.Instance.Crossfade("Overworld",.5f);
        }

        private BattleCharacterData GetAvailableUnit(List<BattleCharacterData> party, ref int currentUnitIndex)
        {
            for (var i = 0; i < party.Count; i++)
            {
                var index = (currentUnitIndex + i) % party.Count;
                if (party[index].CharacterStat.Stat.CurrentHealth <= 0) continue;
                
                var unit = party[index];
                
                //to prevent selected unit selected again
                currentUnitIndex = index + 1;
                return unit;
            }
            
            Debug.LogWarning("No available unit, something went wrong.");
            return null;
        }
    }
}