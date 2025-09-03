using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneController;


public class ReturnToStartScene : MonoBehaviour
{
    public void ExitToStartScene()
    {
        if (SceneController.Instance != null) 
            SceneController.Instance.LoadScene("StartScene");
        else
        {
            SceneManager.LoadScene("StartScene");
            Debug.LogWarning("씬 컨트롤러 인스턴스가 존재하지 않습니다.");
        }
            
    }

}
