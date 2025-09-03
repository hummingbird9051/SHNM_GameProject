using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int totalScore = 0;

    public int GetTotalScore()
    {
        return totalScore;
    }

    private void OnEnable()
    {
        GameEvent.OnScoreGained += HandleScoreGained;
    }

    private void OnDisable()
    {
        GameEvent.OnScoreGained -= HandleScoreGained;
    }

    private void HandleScoreGained(int scoreAmount)
    {
        totalScore += scoreAmount;
        UpdateScoreUI();
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }
}
