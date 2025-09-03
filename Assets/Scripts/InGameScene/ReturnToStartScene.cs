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
            Debug.LogWarning("�� ��Ʈ�ѷ� �ν��Ͻ��� �������� �ʽ��ϴ�.");
        }
            
    }

}
