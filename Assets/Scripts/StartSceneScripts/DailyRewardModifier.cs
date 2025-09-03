using TMPro;
using UnityEngine;

public class DailyRewardModifier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

    void OnEnable()
    {
        tmp.text = (5 - GameDataManager.Instance.GetLivesData()).ToString();
    }

}
