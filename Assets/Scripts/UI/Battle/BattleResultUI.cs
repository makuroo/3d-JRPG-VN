using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
   public class BattleResultUI : MonoBehaviour
   {
      [SerializeField] private Transform _victoryUI;
      [SerializeField] private Transform _defeatUI;
      [SerializeField] private Button _continueButton;


      private void Start()
      {
         _continueButton.onClick.AddListener(() =>
         {
            GameManager.Instance.StartCoroutine(GameManager.Instance.ScreenTransition("Menu"));
         });
      }

      public void ShowResultUI(BattleState battleState)
      {
      
         if (battleState == BattleState.BattleWon)
         {
            gameObject.SetActive(true);
            _victoryUI.transform.localScale = Vector3.zero;
            _victoryUI.gameObject.SetActive(true);
            _victoryUI.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack);
         }

         if (battleState == BattleState.BattleLost)
         {
            gameObject.SetActive(true);
            _defeatUI.transform.localScale = Vector3.zero;
            _defeatUI.gameObject.SetActive(true);
            _defeatUI.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack);
         }
      }
   }
}
