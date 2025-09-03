using UnityEngine;

public class TemporaryMoneySetter : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameDataManager.Instance.PlusMoney(8000);
            GameDataManager.Instance.AddLife();
        }
    }
}
