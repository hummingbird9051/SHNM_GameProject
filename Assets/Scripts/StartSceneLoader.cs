using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneController;

public class StartSceneLoader : MonoBehaviour
{
    private bool isLoading = false;
    private int time = 0;

    void Start()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnLoginSuccess += LoadNextScene;
        }
    }

    void Update()
    {
        if (isLoading)
        {
            time += 1;
            if(time >= 100)
                SceneManager.LoadScene("StartScene");
        }
    }
    private void LoadNextScene()
    {
        if (isLoading)
        {
            return;
        }

        isLoading = true;
        Debug.Log("로딩중");


    }

    void OnDisable()
    {
        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.OnLoginSuccess -= LoadNextScene;
        }
    }
}
