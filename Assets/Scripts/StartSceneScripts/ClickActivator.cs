using UnityEngine;

public class ClickActivator : MonoBehaviour
{
    public GameObject targetObject;


    public void ClickedToActivate()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target Object 없음", this);
            return;
        }

        targetObject.SetActive(true);
        Debug.Log($"Target Object '{targetObject.name}'이 활성화 되었습니다.");

    }

    public void ClickToDeActivate()
    {
        if(targetObject == null)
        {
            Debug.LogWarning("Target Object 없음", this);
            return;
        }
        StartCoroutine(targetObject.GetComponent<PannelController>().DoPopDownAnimation());
        Debug.Log($"Target Object '{targetObject.name}'이 비활성화 되었습니다.");
    }
}
