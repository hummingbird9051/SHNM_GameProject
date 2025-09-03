using TMPro;
using UnityEngine;

public class ScoreShower : MonoBehaviour
{
    private void OnEnable()
    {
        TextMeshProUGUI tmp = gameObject.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = GameDataManager.Instance.GetLastScoreData().ToString();
        }
    }
}
