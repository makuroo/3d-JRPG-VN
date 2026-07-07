using Fungus;
using Unity.Cinemachine;
using UnityEngine;

namespace CustomCommand
{
    [CommandInfo("Cinemachine", 
        "SwitchPriority", 
        "Switch cinemachine camera priority")]
    public class CinemachinePrioritySwitch : Command
    {
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private int _newPriority;
        
        public override void OnEnter()
        {
            if (!_cinemachineCamera)
            {
                Debug.LogError("CinemachineCamera not found");
                Continue();
                return;
            }
        
            base.OnEnter();
            _cinemachineCamera.Priority = _newPriority;
        
            Continue();
        }
        
        public override string GetSummary()
        {
            if (!_cinemachineCamera)
            {
                return "Error: No Camera Selected";
            }
            return $"Set {_cinemachineCamera.name} priority to {_newPriority}";
        }
        
        public override Color GetButtonColor()
        {
            return Color.aquamarine; 
        }
    }
}
