using System;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static event Action<int> OnScoreGained;
    public static event Action<bool> IsGameOver;

    public static void RaiseGameOver()
    {
        IsGameOver?.Invoke(true);
    }

    public static void RaiseScoreGained(int scoreAmount)
    {
        OnScoreGained?.Invoke(scoreAmount);
    }
}
