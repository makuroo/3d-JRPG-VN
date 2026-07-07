using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public enum StoryState
    {
        Intro,
        GatherInfo,
        TalkedWithGuard
    }

    [Serializable]
    public struct StoryStateInfo
    {
        public StoryState State;
        public string BlockName;
        public bool ShouldShowWarning;
    }

    public class StoryManager : MonoBehaviour
    {
        public static StoryManager Instance;
        private StoryState _currentState = StoryState.Intro;
        [ShowInInspector,ReadOnly]
        public StoryState CurrentState => _currentState;
        public Action<StoryState> OnStateChange;

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

        //Mostly called from fungus
        public void ChangeState(StoryState newState)
        {
            _currentState = newState;
            OnStateChange?.Invoke(newState);
        }
    }
}

