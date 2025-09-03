using UnityEngine;
using System.Collections;

public class PannelController : MonoBehaviour
{
    public GameObject childScreenObject;


    public IEnumerator DoPopDownAnimation()
    {
        yield return StartCoroutine(childScreenObject.GetComponent<PopUpAnimation>().DoPopDownAnimation());

        gameObject.SetActive(false);
    }
}
