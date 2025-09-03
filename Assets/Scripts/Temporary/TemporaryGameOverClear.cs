using UnityEngine;

public class TemporaryGameOverClear : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            manager.GameOver();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            manager.GameClear();
        }
    }
}
