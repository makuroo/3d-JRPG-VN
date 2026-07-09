using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        ActionPhase,
        BattleWon,
        BattleLost
    }

    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;
        public Action<BattleState> OnStateChange;
        public Action<CharacterDataSo> OnPrepareBattle;
        public Action<List<BattleCharacterData>> OnPartyInitialized;
        [ShowInInspector,ReadOnly]
        private BattleState _currentState = BattleState.BattleStart;

        private CharacterDataSo _pickedUnit;
        
        public BattleState CurrentState => _currentState;
        public BattleCharacterData ActiveUnit { get; set; }

        public BattleActionSO SelectedBattleAction => _selectedBattleAction;

        public CharacterDataSo PickedUnit
        {
            get => _pickedUnit;
            set => _pickedUnit = value;
        }

        private List<BattleCharacterData> _playerPartyData = new();
        private List<BattleCharacterData> _enemyPartyData = new();
    
        private BattleActionSO _selectedBattleAction;
        private int _currentPlayerIndex;
        private int _currentEnemyIndex;
        
        private List<BattleCharacterData> _sortedTurnOrder = new();

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

        public void SetBattle(CharacterDataSo playerData)
        {
            ScreenFader.Instance.FadeIn(.5f, () =>
            {
                StartCoroutine(TransitionCoroutine(playerData));
            });
        }
    
        private IEnumerator TransitionCoroutine(CharacterDataSo playerData)
        {
            var handler = SceneManager.LoadSceneAsync("Battle");
            yield return handler;
            GameManager.Instance.UpdateState(GameState.Battle);
            //AudioManager.Instance.Crossfade("Battle",.5f);
            OnPrepareBattle?.Invoke(playerData);
            ScreenFader.Instance.FadeOut(.5f);
        }

        public void InitializeParty(List<BattleCharacterData> playerParty, List<BattleCharacterData> enemyParty)
        {
            _playerPartyData = playerParty;
            _enemyPartyData = enemyParty;

            foreach (var player in playerParty)
            {
                player.CharacterCombat.OnDeath += RemoveUnitFromQueue;
            }
            
            OnPartyInitialized?.Invoke(playerParty);
            OnStateChange?.Invoke(BattleState.ActionPhase);
            _currentState = BattleState.ActionPhase;
            GenerateTurnOrder();
            ProcessNextTurn();
        }

        private void SetSelectedUnit(BattleCharacterData unit)
        {
            if (ActiveUnit != null)
            {
                ActiveUnit.BattleCharacterView.FocusOnUnit(false);
            }
        
            ActiveUnit = unit;
            if (unit.Team == Team.Player)
            {
                unit.BattleCharacterView.FocusOnUnit(true);
            }
            Debug.Log(unit.CharacterStat.BaseData.CharacterName,ActiveUnit.BattleCharacterView);
            BattleUIManager.Instance.OnUnitSelected?.Invoke(ActiveUnit);
        }
    
        public void OnActionSelected(BattleActionSO action)
        {
            _selectedBattleAction = action;
        
            if(ActiveUnit.Team != Team.Player) return;
            //BattleUIManager.Instance.ToggleCursor(true);
            foreach (var unit in _enemyPartyData)
            {
                if (unit.Stat.CurrentHealth > 0)
                {
                    Debug.Log($"Enable {unit.CharacterDataSo.CharacterName} Selectable",unit.BattleCharacterView);
                    unit.BattleCharacterView.SetSelectable(true);
                }
            }
        }

        private IEnumerator BattleIntroCoroutine()
        {
            yield return new WaitForSeconds(1f);
            ProcessNextTurn();
        }

        public void ExecuteActionCommand(BattleCharacterData target)
        {
            //BattleUIManager.Instance.ToggleCursor(false);
            foreach (var partyUnit in _enemyPartyData)
            {
                partyUnit.BattleCharacterView.SetSelectable(false);
                partyUnit.BattleCharacterView.FocusOnUnit(false);
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
            //Debug.Log(Time.time);
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
            _sortedTurnOrder.Remove(ActiveUnit);
            ProcessNextTurn();
            //Debug.Log(Time.time);
        }

        private void ProcessNextTurn()
        {
            if (_sortedTurnOrder.Count == 0)
            {
                GenerateTurnOrder();
            }
            
            var unit = _sortedTurnOrder[0];
            SetSelectedUnit(unit);

           if(unit.Team == Team.Enemy)
            {
                ActiveUnit.CharacterStat.GetComponent<EnemyCombat>().PickCombatMove();
            }
        }

        public BattleCharacterData GetRandomPlayerUnit()
        {
            var aliveAlly = _playerPartyData.Where(x => x.Stat.CurrentHealth > 0).ToList();
            var pickedAlly = aliveAlly[Random.Range(0,aliveAlly.Count)];
            return pickedAlly;
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

        private void GenerateTurnOrder()
        {
            var order = new List<BattleCharacterData>();
            order.AddRange(_playerPartyData);
            order.AddRange(_enemyPartyData);
            var sorted = order.Where(x => x.CharacterStat.Stat.CurrentHealth > 0)
                .OrderByDescending(x => x.CharacterStat.Stat.Speed);
            _sortedTurnOrder = sorted.ToList();
        }

        private void RemoveUnitFromQueue(BattleCharacterData unit)
        {
            Debug.Log(unit.CharacterStat.BaseData.CharacterName);
            _sortedTurnOrder.Remove(unit);
        }
    }
}