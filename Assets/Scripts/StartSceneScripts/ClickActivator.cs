using UnityEngine;

public class ClickActivator : MonoBehaviour
{
    public GameObject targetObject;


    public void ClickedToActivate()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target Object ����", this);
            return;
        }

        targetObject.SetActive(true);
        Debug.Log($"Target Object '{targetObject.name}'�� Ȱ��ȭ �Ǿ����ϴ�.");

    }

    public void ClickToDeActivate()
    {
        if(targetObject == null)
        {
            Debug.LogWarning("Target Object ����", this);
            return;
        }
        StartCoroutine(targetObject.GetComponent<PannelController>().DoPopDownAnimation());
        Debug.Log($"Target Object '{targetObject.name}'�� ��Ȱ��ȭ �Ǿ����ϴ�.");
    }
}
