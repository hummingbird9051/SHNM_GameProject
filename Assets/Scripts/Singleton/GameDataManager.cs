using UnityEngine;
using System;
using Firebase.Firestore;

public class GameDataManager : SingletonBase<GameDataManager>
{

    public event Action<int> OnLivesChanged;
    public event Action<int> OnMoneyChanged;

    private int _playerLives;

    private int _playerMoney;

    private int _playerScore;

    private bool _isCleared;

    private int _lastPlayerScore = 0;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
    void Start()
    {
        FirebaseManager.Instance.OnDataLoaded += InitializeData;
    }

    private void InitializeData(PlayerData data)
    {
        Debug.Log($"<color=yellow>[GameDataManager]</color> InitializeData 실행 Money: {data.Money}");
        _playerMoney = data.Money;
        _playerLives = data.Lives;
        _playerScore = data.HighScore;
        _isCleared = data.IsCleared;

        Debug.Log("Firebase실행");

        OnLivesChanged?.Invoke(_playerLives);
        OnMoneyChanged?.Invoke(_playerMoney);
    }

    public void SaveAllData()
    {
        PlayerData dataToSave = new PlayerData
        {
            Lives = _playerLives,
            Money = _playerMoney,
            HighScore = _playerScore,
            IsCleared = _isCleared
        };
        FirebaseManager.Instance.SavePlayerData(dataToSave);
    }

    //playerLives -------------------------------------

    private void SaveLives()
    {
        OnLivesChanged?.Invoke(_playerLives);
        SaveAllData();
    }

    public void SetLivesData(int newLives)
    {
        if (newLives < 0) newLives = 0;

        _playerLives = newLives;
        SaveLives();
        Debug.Log($"수강권 갯수: {_playerLives}");
    }

    public int GetLivesData() => _playerLives;

    public void AddLife(int amount = 1)
    {
        if(amount < 0) return;

        _playerLives += amount;

        SaveLives();
        Debug.Log($"수강권 증가: {amount}, 최종 수강권: {_playerLives}");
    }

    public void LoseLife(int amount = 1)
    {
        if (amount < 0) return;

        if (_playerLives == 0)
        {
            Debug.Log("수강권이 없습니다");
            return;
        }
        _playerLives -= amount;

        SaveLives();
    }

    //playerLives --------------------------------------------------

    //playerScore --------------------------------------------------
    
    private void SaveScore()
    {
        SaveAllData();
    }

    public void SetScoreData(int newScore)
    {
        if (newScore < 0 ) newScore = 0;
        if (_playerScore > newScore) return;
        _playerScore = newScore;

        SaveScore();
        Debug.Log($"기록을 갱신했습니다: {newScore}");
    }

    public int GetScoreData() => _playerScore;

    //playerScore -----------------------------------------------------------


    //lastPlayerScore --------------------------------------
    public int GetLastScoreData()
    {
        return _lastPlayerScore;
    }

    public void SetLastScoreData(int score)
    {
        Debug.Log($"마지막 점수 : {score}");
        _lastPlayerScore = score;
        Debug.Log(_lastPlayerScore);
    }
    // lastPlayerScore ----------------------------------------------------
    
    //playerMoney -------------------------------------------------------------
    public void SaveMoney()
    {
        SaveAllData();
        OnMoneyChanged?.Invoke(_playerMoney);
    }

    public bool MinusMoney(int money)
    {
        if (money > _playerMoney) return false;
        _playerMoney -= money;
        SaveMoney();
        return true;
    }

    public void PlusMoney(int money)
    {
        _playerMoney += money;
        SaveMoney();
    }

    public int GetMoney() => _playerMoney;

    public void SetCleared()
    {
        _isCleared = true;
        SaveAllData();
    }
}
