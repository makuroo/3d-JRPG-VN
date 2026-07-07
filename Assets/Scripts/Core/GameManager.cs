using System;
using UnityEngine;

namespace Core
{
    public enum GameState
    {
        Exploration,
        Battle,
        Cutscene
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Action<GameState> OnGameStateChange;
        private GameState _currentState;
        public GameState CurrentState => _currentState;

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

        public void UpdateState(GameState newState)
        {
            if (_currentState == newState) return;
            OnGameStateChange?.Invoke(newState);
            _currentState = newState;
        }
        
    }
}