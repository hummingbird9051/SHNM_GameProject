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
            Debug.Log($"�� �ε� ��û: {sceneName}");
        }
        else
        {
            Debug.LogError($"�� '{sceneName}'�� ã�� �� �����ϴ�.");
        }
    }

    public System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        if(Application.CanStreamedLevelBeLoaded(sceneName))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while(!asyncLoad.isDone)
            {
                Debug.Log($"�� �ε� �����: {asyncLoad.progress * 100}%");
                yield return null;
            }
            Debug.Log($"�� �ε� �Ϸ�: {sceneName}");
        }
        else
        {
            Debug.Log($"�� '{sceneName}'�� ã�� �� �����ϴ�.");
        }
    }

    public System.Collections.IEnumerator UnloadSceneAsync(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).IsValid())
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

            while ( !asyncUnload.isDone)
            {
                Debug.Log($"�� ��ε� ����� : {asyncUnload.progress * 100}%");
                yield return null;
            }
            Debug.Log($"�� ��ε� �Ϸ� : {sceneName}");
        }
        else
        {
            Debug.LogWarning($"�� '{sceneName}'�� ���� �ε�Ǿ� ���� �ʽ��ϴ�.");
        }
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

}
