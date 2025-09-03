using System;
using System.Collections;
using UnityEngine;

public class StartSceneLoginChecker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DailyRewardManager.Instance.CheckDailyLogin(FirebaseManager.Instance.CurrentPlayerData.Value);
        Debug.Log($"{FirebaseManager.Instance.CurrentPlayerData.Value.LastLogin}");
    }

    private void Update()
    {
        StartCoroutine(CheckDayChanged());
    }

    IEnumerator CheckDayChanged()
    {
        yield return new WaitForSeconds(60f);
        DailyRewardManager.Instance.CheckDailyLogin(FirebaseManager.Instance.CurrentPlayerData.Value);
    }


}
