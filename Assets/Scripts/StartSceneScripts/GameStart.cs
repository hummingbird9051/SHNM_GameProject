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
                Debug.LogWarning("�� ��Ʈ�ѷ� �ν��Ͻ��� �������� �ʽ��ϴ�.");
            }
            GameDataManager.Instance.LoseLife();
        }
        else
        {
            targetObject.SetActive(true);
            Debug.Log("��밡���� �������� �����ϴ�.");
        }
        
    }
}
