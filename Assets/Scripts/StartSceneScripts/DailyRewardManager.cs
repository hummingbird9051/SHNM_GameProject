using UnityEngine;
using Firebase.Firestore;
using System;

public class DailyRewardManager : SingletonBase<DailyRewardManager>
{
    [SerializeField] private GameObject dailyPopUpUI;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnDataLoaded += CheckDailyLogin;
            if (FirebaseManager.Instance.CurrentPlayerData.HasValue)
            {
                Debug.Log("정보를 불러옵니다");
                CheckDailyLogin(FirebaseManager.Instance.CurrentPlayerData.Value);
            }
            else
            {
                Debug.Log("정보가 없습니다");
            }
        }
    }

    public void CheckDailyLogin(PlayerData data)
    {
        if(!FirebaseManager.Instance.CurrentPlayerData.HasValue)
        {
            Debug.Log("Skip the Attendance check because there isn't the Player data.");
            return;
        }


        try
        {
            // Load Korea Standard time from TimeSonInfo Class
            TimeZoneInfo kstZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");

            // Convert last Utc time to Kst
            DateTime lastLoginUtc = data.LastLogin.ToDateTime();
            DateTime lastLoginKst = TimeZoneInfo.ConvertTimeFromUtc(lastLoginUtc, kstZone);

            // Convert today's Utc to Kst
            DateTime todayUtc = DateTime.UtcNow;
            DateTime todayKst = TimeZoneInfo.ConvertTimeFromUtc(todayUtc, kstZone);

            if (lastLoginKst.Date < todayKst.Date)
            {
                Debug.Log("날짜 변경(KST 기준 자정), 보상 지급");
                GameDataManager.Instance.SetLivesData(5);
                dailyPopUpUI.SetActive(true);
                FirebaseManager.Instance.UpdateLastLoginTime();
            }
            else
            {
                Debug.Log("오늘 이미 로그인한 이력이 있습니다 (KST 기준 자정)");
            }
        }
        catch (TimeZoneNotFoundException)
        {
            Debug.LogError("Could not find the Korea Standard Time zone.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred during daily login check: {ex.Message}");
        }

    }

    void OnDestroy()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnDataLoaded -= CheckDailyLogin;
        }
    }
}