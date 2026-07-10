using System;
using DG.Tweening;
using Fungus;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum DialgoueMode
{
    Auto,
    Fast,
    Manual,
}

namespace Core
{
    public class DialogueManager : MonoBehaviour
    {
        private DialgoueMode _currentMode = DialgoueMode.Manual;
        [SerializeField] private Button _autoButton;
        [SerializeField] private Button _fastButton;
        [SerializeField] private float _waitTime;
        private float _autoTimer;
        private Writer _writer;
        private DialogInput _dialogInput;
   
        private void OnEnable()
        {
            BlockSignals.OnBlockEnd += ResetMode;
            _autoButton.onClick.AddListener(() =>
            {
                SetMode(DialgoueMode.Auto);
            });
            
            _fastButton.onClick.AddListener(() =>
            {
                SetMode(DialgoueMode.Fast);
            });
        }

        private void OnDisable()
        {
            _autoButton.onClick.RemoveAllListeners();
            _fastButton.onClick.RemoveAllListeners();
            BlockSignals.OnBlockEnd -= ResetMode;
        }

        private void Update()
        {
            switch (_currentMode)
            {
                case DialgoueMode.Auto:
                    if (_writer.IsWaitingForInput)
                    {
                        _autoTimer += Time.deltaTime;
                        if (_autoTimer >= _waitTime)
                        {
                            _dialogInput.SetNextLineFlag();
                            _autoTimer = 0;
                        }
                    }
                    else
                    {
                        _autoTimer = 0;
                    }
                    break;
                case DialgoueMode.Fast:
                    bool isCurrentlyTyping = _writer.IsWriting && !_writer.IsWaitingForInput;
                    if (isCurrentlyTyping)
                    {
                        _dialogInput.SetNextLineFlag();
                    }
                    
                    if (_writer.IsWaitingForInput)
                    {
                        _autoTimer += Time.deltaTime;
                        if (_autoTimer >= .5f)
                        {
                            _dialogInput.SetNextLineFlag();
                            _autoTimer = 0;
                        }
                    }
                    else
                    {
                        _autoTimer = 0;
                    }
                    break;
            }
        }
        
        private void SetMode(DialgoueMode mode)
        {
            GetDialogueComponent();
            _currentMode = mode;
        }
        
        private void GetDialogueComponent()
        {
            if (!_writer)
            {
                _writer = SayDialog.ActiveSayDialog.GetComponent<Writer>();
            }

            if (!_dialogInput)
            {
                _dialogInput = SayDialog.ActiveSayDialog.GetComponent<DialogInput>();
            }
        }
        
        private void ResetMode(Block block)
        {
            _currentMode = DialgoueMode.Manual;
        }
    }
}
