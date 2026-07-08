using System;
using Core;
using Data.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingUnitButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _unitName;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Setup(CharacterDataSo pickedUnit)
    {
        _unitName.text = pickedUnit.CharacterName;
        _button.onClick.AddListener(() =>
        {
            PartyManager.Instance.InitializePartyData(pickedUnit);
            gameObject.SetActive(false);
            BattleManager.Instance.SetBattle(pickedUnit);
        });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}
