using System;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

namespace Core
{
    public struct HistoryData
    {
        public string CharacterName;
        public string Dialog;
    }
    
    public class DialogHistoryManager : MonoBehaviour
    {
        public static DialogHistoryManager instance;
        private List<HistoryData> _historyData = new();

        public List<HistoryData> HistoryData => _historyData;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            BlockSignals.OnCommandExecute += RecordHistory;
            BlockSignals.OnBlockEnd += ClearHistoryOnBlockCompleted;
        }

        private void RecordHistory(Block block, Command command, int commandIndex, int maxCommandIndex)
        {
            Say sayCommand = null;
            string processedText = null;
            if (command is Say say)
            {
                sayCommand = say;
                var rawText = sayCommand.GetStandardText();
                processedText = sayCommand.GetFlowchart().SubstituteVariables(rawText);
            }

            var dialogHistory = new HistoryData();
            if (sayCommand != null)
            {
                dialogHistory.CharacterName = sayCommand._Character.NameText;
                dialogHistory.Dialog = processedText;
            }

            _historyData.Add(dialogHistory);
        }

        private void ClearHistoryOnBlockCompleted(Block block)
        {
            HistoryData.Clear();
        }
    }
}
