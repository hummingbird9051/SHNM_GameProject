using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : ClickActivator
{
    public void StartGame()
    {
        if(GameDataManager.Instance.GetLivesData() > 0 )
        {
            if (SceneController.Instance != null)
            {
                SceneController.Instance.LoadScene("InGameScene");
            }
            else
            {
                SceneManager.LoadScene("InGameScene");
                Debug.LogWarning("씬 컨트롤러 인스턴스가 존재하지 않습니다.");
            }
            GameDataManager.Instance.LoseLife();
        }
        else
        {
            targetObject.SetActive(true);
            Debug.Log("사용가능한 수강권이 없습니다.");
        }
        
    }
}
