using Firebase;
using Firebase.AI;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[FirestoreData]
public struct PlayerData
{
    [FirestoreProperty]
    public int Lives { get; set; }

    [FirestoreProperty]
    public int Money { get; set; }

    [FirestoreProperty]
    public int HighScore { get; set; }

    [FirestoreProperty]
    public bool IsCleared { get; set; }

    [FirestoreProperty]
    public Timestamp LastLogin { get; set; }
}

public class FirebaseManager : SingletonBase<FirebaseManager>
{
    public bool IsFirebaseInitialized { get; private set; } = false;

    public event Action OnAuthReady;
    public event Action<PlayerData> OnDataLoaded;
    public event Action OnLoginSuccess;

    private bool isAuthReadyToSignal = false;
    private bool isLoginSucceeded = false;

    private bool isResultReady = false;
    private bool actionSuccessResult;
    private string actionMessageResult;
    private Action<bool, string> onActionResultCallback;

    public FirebaseAuth auth;
    public FirebaseFirestore db;
    public FirebaseUser user;

    public PlayerData? CurrentPlayerData { get; private set; } = null;

    [SerializeField] private GameObject accountLinkUI;

    protected override void Awake()
    {
        base.Awake();

        InitializeFirebase();
    }


    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;
                IsFirebaseInitialized = true;
                Debug.Log("Firebase 초기화");

                if (auth.CurrentUser != null)
                {
                    user = auth.CurrentUser;
                    Debug.Log($"로그인 성공! 정보: {user.Email} (ID: {user.UserId})");
                    LoadPlayerData(); 
                    isLoginSucceeded = true; 
                }
                else
                {
                    Debug.Log("로그인 정보가 없어 대기.");
                }

                isAuthReadyToSignal = true; 
            }
            else
            {
                Debug.LogError($"알수 없는 오류가 발생했습니다: {dependencyStatus}");
            }
        });
    }

    void Update()
    {
        if (isAuthReadyToSignal)
        {
            isAuthReadyToSignal = false;
            OnAuthReady?.Invoke(); 
        }

        if (isLoginSucceeded)
        {
            isLoginSucceeded = false;
            OnLoginSuccess?.Invoke();
        }

        if (isResultReady)
        {
            isResultReady = false;
            onActionResultCallback?.Invoke(actionSuccessResult, actionMessageResult);
        }
    }

    public void SavePlayerData(PlayerData dataToSave)
    {
        if (user == null)
        {
            Debug.LogError("플레이어 정보가 없습니다");
            return;
        }

        DocumentReference docRef = db.Collection("players").Document(user.UserId);

        // SetAsync에 MergeAll 옵션을 주면, dataToSave에 없는 필드(LastLogin)는 건드리지 않고
        // 존재하는 필드(Lives, Money 등)만 업데이트합니다.
        docRef.SetAsync(dataToSave, SetOptions.MergeAll);

        // 로컬 캐시도 업데이트합니다.
        if (CurrentPlayerData.HasValue)
        {
            PlayerData currentData = CurrentPlayerData.Value;
            currentData.Lives = dataToSave.Lives;
            currentData.Money = dataToSave.Money;
            currentData.HighScore = dataToSave.HighScore;
            currentData.IsCleared = dataToSave.IsCleared;
            CurrentPlayerData = currentData;
        }
    }

    public async void LoadPlayerData()
    {
        if (user == null)
        {
            Debug.LogError("플레이어 정보가 없습니다");
            return;
        }

        DocumentReference docRef = db.Collection("players").Document(user.UserId);

        try
        {
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            DateTime utcNow = DateTime.UtcNow;
            Timestamp nowTimestamp = Timestamp.FromDateTime(utcNow);
            PlayerData dataToProcess;

            if (snapshot.Exists)
            {
                PlayerData loadedData = snapshot.ConvertTo<PlayerData>();
                DateTime lastLoginDate = loadedData.LastLogin.ToDateTime();

                DateTime startOfLastWeek = GetStartOfWeek(lastLoginDate, DayOfWeek.Monday);
                DateTime startOfCurrentWeek = GetStartOfWeek(utcNow, DayOfWeek.Monday);

                if (startOfCurrentWeek > startOfLastWeek)
                {
                    Debug.Log("한 주가 새로 시작하여 정보를 초기화합니다.");
                    loadedData.IsCleared = false;
                    await docRef.SetAsync(loadedData); 
                }
                else
                {
                    await docRef.UpdateAsync("LastLogin", nowTimestamp);
                }
                dataToProcess = loadedData;
            }
            else
            {
                Debug.Log("플레이어 정보가 없어 새로 생성합니다");
                PlayerData defaultData = new PlayerData
                {
                    Lives = 5,
                    Money = 0,
                    HighScore = 0,
                    IsCleared = false,
                    LastLogin = nowTimestamp
                };
                await docRef.SetAsync(defaultData);
                dataToProcess = defaultData;
            }

            CurrentPlayerData = dataToProcess;
            Debug.Log($"<color=cyan>[FirebaseManager]</color> OnDataLoaded 실행. IsCleared: {dataToProcess.IsCleared}");
            OnDataLoaded?.Invoke(dataToProcess);
        }
        catch (Exception e)
        {
            Debug.LogError($"알 수 없는 오류가 발생했습니다: {e}");
        }
    }

    public void SignUpWithEmail(string email, string password, Action<bool, string> onResult)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                UpdateWithError(task, onResult);
                return;
            }

            user = task.Result.User;
            Debug.Log($"로그인 성공! : {user.Email}");

            onActionResultCallback = onResult;
            actionSuccessResult = true;
            actionMessageResult = "로그인 성공!";
            isResultReady = true;
            isLoginSucceeded = true;

            LoadPlayerData();
        });
    }

    public void SignInWithEmail(string email, string password, Action<bool, string> onResult)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                UpdateWithError(task, onResult);
                return;
            }

            user = task.Result.User;
            Debug.Log($"로그인 성공!: {user.Email}");

            onActionResultCallback = onResult;
            actionSuccessResult = true;
            actionMessageResult = "로그인 성공!";
            isLoginSucceeded = true;

            LoadPlayerData();
        });
    }

    private void UpdateWithError(System.Threading.Tasks.Task task, Action<bool, string> onResult)
    {
        onActionResultCallback = onResult;
        actionSuccessResult = false;

        // Firebase ¿¹¿Ü ºÐ¼®
        if (task.Exception != null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            if (firebaseEx != null)
            {
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.EmailAlreadyInUse:
                        actionMessageResult = "이미 사용 중인 이메일입니다.";
                        break;
                    case AuthError.WrongPassword:
                        actionMessageResult = "비밀번호가 틀렸습니다.";
                        break;
                    case AuthError.UserNotFound:
                        actionMessageResult = "가입되지 않은 이메일입니다.";
                        break;
                    case AuthError.InvalidEmail:
                        actionMessageResult = "유효하지 않은 이메일 형식입니다";
                        break;
                    case AuthError.WeakPassword:
                        actionMessageResult = "비밀번호는 6자리 이상이어야 합니다.";
                        break;
                    default:
                        actionMessageResult = "알 수 없는 오류가 발생했습니다.";
                        break;
                }
            }
        }
        else
        {
            actionMessageResult = "알 수 없는 오류가 발생했습니다.";
        }

        isResultReady = true;
    }

    private DateTime GetStartOfWeek(DateTime date, DayOfWeek startOfWeek)
    {
        int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    public void UpdateLastLoginTime()
    {
        if (auth.CurrentUser == null) return;

        string userId = auth.CurrentUser.UserId;
        DocumentReference docRef = db.Collection("players").Document(userId);

        // 1. 서버의 "LastLogin" 필드만 현재 시간으로 업데이트합니다.
        docRef.UpdateAsync("LastLogin", Timestamp.GetCurrentTimestamp()).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to update LastLogin time on server: " + task.Exception);
                return;
            }
            Debug.Log("LastLogin time successfully updated on server.");
        });

        // 2. 로컬에 캐싱된 데이터도 갱신하여 데이터 일관성을 유지합니다.
        if (CurrentPlayerData.HasValue)
        {
            PlayerData updatedData = CurrentPlayerData.Value;
            updatedData.LastLogin = Timestamp.GetCurrentTimestamp();
            CurrentPlayerData = updatedData;
        }
    }
}
