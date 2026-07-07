using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class DamagePopup : MonoBehaviour
    {
       [SerializeField] private TMP_Text _damageText;

        public void Initialize(string damage)
        {
            _damageText.text = damage;
            transform.DOPunchPosition(Vector3.up, 1, 1,1.5f);
        }
    }
}
