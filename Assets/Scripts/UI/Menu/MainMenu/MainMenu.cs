using Core;
using UnityEngine;

namespace UI.Menu.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Transform _pickUnitUI;

        public void PickBattleMode()
        {
            GameManager.Instance.UpdateGameMode(GameMode.Battle);
            _pickUnitUI.gameObject.SetActive(true);
        }

        public void PickExplorationMode()
        {
            GameManager.Instance.UpdateGameMode(GameMode.Exploration);
            Debug.Log("Exploration Mode");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
