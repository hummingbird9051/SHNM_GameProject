using UnityEngine;
using TMPro;
public class MoneyTextUpdater : MonoBehaviour
{

    private TextMeshProUGUI myTextMeshProText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (myTextMeshProText == null) myTextMeshProText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (GameDataManager.Instance != null)
        {
            Debug.Log("<color=lime>[MoneyTextUpdater]</color> OnEnable 실행.");
            GameDataManager.Instance.OnMoneyChanged += UpdateMoneyDisplay;
            UpdateMoneyDisplay(GameDataManager.Instance.GetMoney());
        }
        else
        {
            Debug.LogError("GameDataManager 실행");
        }


    }

    void OnDisable()
    {
        if (GameDataManager.Instance != null) GameDataManager.Instance.OnMoneyChanged -= UpdateMoneyDisplay;
    }

    private void UpdateMoneyDisplay(int newMoney)
    {
        if (myTextMeshProText != null)
        {
            Debug.Log($"<color=lime>[MoneyTextUpdater]</color> UpdateMoneyDisplay 실행: {newMoney}");
            myTextMeshProText.text = $"{newMoney}";
        }
    }
}