using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SingletonBase<SceneController>
{


    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadScene(string sceneName)
    {
        if(Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            Debug.Log($"씬 로드 요청: {sceneName}");
        }
        else
        {
            Debug.LogError($"씬 '{sceneName}'을 찾을 수 없습니다.");
        }
    }

    public System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        if(Application.CanStreamedLevelBeLoaded(sceneName))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while(!asyncLoad.isDone)
            {
                Debug.Log($"씬 로딩 진행률: {asyncLoad.progress * 100}%");
                yield return null;
            }
            Debug.Log($"씬 로드 완료: {sceneName}");
        }
        else
        {
            Debug.Log($"씬 '{sceneName}'을 찾을 수 없습니다.");
        }
    }

    public System.Collections.IEnumerator UnloadSceneAsync(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).IsValid())
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            while ( !asyncUnload.isDone)
            {
                Debug.Log($"씬 언로딩 진행률 : {asyncUnload.progress * 100}%");
                yield return null;
            }
            Debug.Log($"씬 언로드 완료 : {sceneName}");
        }
        else
        {
            Debug.LogWarning($"씬 '{sceneName}'은 현재 로드되어 있지 않습니다.");
        }
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

}
