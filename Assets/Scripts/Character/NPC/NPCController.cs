using System.Collections.Generic;
using Core;
using Fungus;
using Interface;
using UnityEngine;
using static System.String;

namespace Character.NPC
{
    public class NPCController : MonoBehaviour,IInteractable
    {
        [SerializeField] private GameObject _interactUI;
        [Header("Story Collection")]
        [SerializeField] private List<StoryStateInfo> _storyInfos = new List<StoryStateInfo>();

        private string _currStoryBlockName;
        private void OnEnable()
        {
            if(StoryManager.Instance != null)
                StoryManager.Instance.OnStateChange += UpdateBlockName;
            //_interactUI?.SetActive(false);
        }

        private void OnDisable()
        {
            if(StoryManager.Instance !=null)
                StoryManager.Instance.OnStateChange -= UpdateBlockName;
        }

        public void Interact()
        {
            Debug.Log(_currStoryBlockName);
            if(IsNullOrEmpty(_currStoryBlockName)) return;
            Flowchart.BroadcastFungusMessage(_currStoryBlockName); 
           // _interactUI?.SetActive(false);
        }

        private void UpdateBlockName(StoryState state)
        {
            foreach  (var storyInfo in _storyInfos)
            {
                if (storyInfo.State != state) continue;
                
                _currStoryBlockName = storyInfo.BlockName;
                _interactUI.SetActive(storyInfo.ShouldShowWarning);
                return;
            }
            _interactUI.SetActive(false);
        }
    }
}

