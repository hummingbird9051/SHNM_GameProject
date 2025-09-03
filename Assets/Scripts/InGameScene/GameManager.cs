using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    public void GameOver()
    {
        SaveData();
        SceneController.Instance.LoadScene("GameOverScene");
    }

    public void GameClear()
    {
        SaveData();
        GameDataManager.Instance.SetCleared();
        SceneController.Instance.LoadScene("GameClearScene");
    }

    private void SaveData()
    {
        GameDataManager.Instance.PlusMoney(scoreManager.GetTotalScore());
        GameDataManager.Instance.SetScoreData(scoreManager.GetTotalScore());
        GameDataManager.Instance.SetLastScoreData(scoreManager.GetTotalScore());
    }
}
