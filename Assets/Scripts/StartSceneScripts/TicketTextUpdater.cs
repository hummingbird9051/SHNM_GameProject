using UnityEngine;
using TMPro;
public class TicketTextUpdater : MonoBehaviour
{

    private TextMeshProUGUI myTextMeshProText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        if (myTextMeshProText == null) myTextMeshProText = transform.gameObject.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if(GameDataManager.Instance != null)
        {
            GameDataManager.Instance.OnLivesChanged += UpdateLivesDisplay;
            UpdateLivesDisplay(GameDataManager.Instance.GetLivesData());
        }
        else
        {
            Debug.LogError("GameDataManager 인스턴스가 없습니다.");
        }
        

    }

    void OnDisable()
    {
        if (GameDataManager.Instance != null) GameDataManager.Instance.OnLivesChanged -= UpdateLivesDisplay;
    }

    private void UpdateLivesDisplay(int newLives)
    {
        if(myTextMeshProText != null)
        {
            myTextMeshProText.text = $"{newLives}";
        }
    }
}
