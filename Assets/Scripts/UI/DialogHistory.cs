using Core;
using TMPro;
using UnityEngine;

public class DialogHistory : MonoBehaviour
{
    [SerializeField] private Transform _historyUI;
    [SerializeField] private Transform _textUIPrefab;
    [SerializeField] private Transform _textUIContainer;

    public void ToggleHistoryWindow()
    {
        _historyUI.gameObject.SetActive(!_historyUI.gameObject.activeSelf);
        if (_historyUI.gameObject.activeSelf)
        {
            _historyUI.gameObject.SetActive(true);
            PopulateHistory();
        }
        else
        {
            _historyUI.gameObject.SetActive(false);
        }
    }

    private void PopulateHistory()
    {
        var text = _historyUI.GetComponentsInChildren<TMP_Text>();
        foreach (var child in text)
        {
            Destroy(child);
        }
        
        var data = DialogHistoryManager.instance.HistoryData;
        foreach (var history in data)
        {
           var spawned = Instantiate(_textUIPrefab, _textUIContainer);
            spawned.GetComponent<TMP_Text>().text = $"{history.CharacterName} : {history.Dialog}";
        }
    }
}
