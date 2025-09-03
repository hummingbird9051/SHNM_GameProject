using UnityEngine;
using System.Collections;

public class PopUpAnimation : MonoBehaviour
{
    public float animationDuration = 0.1f;
    private Vector3 minScale = new Vector3(0f, 0f, 1f);
    private Vector3 maxScale = new Vector3(1f, 1f, 1f);

    private Coroutine currentAnimationCoroutine;

    void OnEnable()
    {
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
        }

        currentAnimationCoroutine = StartCoroutine(DoPopUpAnimation());
    }

    void OnDisable()
    {
        if(currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }
    }

    IEnumerator DoPopUpAnimation()
    {
        transform.localScale = minScale;
        float timer = 0f;

        while (timer < animationDuration)
        {
            float t = timer / animationDuration;

            transform.localScale = Vector3.Lerp(minScale, maxScale, t);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = maxScale;
        Debug.Log("팝업 애니메이션 완료");
    }

    public IEnumerator DoPopDownAnimation()
    {
        float timer = 0f;
        Vector3 startScale = transform.localScale;

        while(timer < animationDuration)
        {
            float t = timer / animationDuration;
            transform.localScale = Vector3.Lerp(startScale, minScale, t);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = minScale;
        Debug.Log("팝다운 애니메이션 완료");
    }
}
