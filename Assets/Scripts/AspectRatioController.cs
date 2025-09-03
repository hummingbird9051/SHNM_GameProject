using UnityEngine;


[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    // 목표 화면 비율 (16:9)
    public float targetAspect = 16.0f / 9.0f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateCrop();
    }

    // 화면 크기가 변경될 때마다 호출될 수 있도록 Update에 넣거나,
    // 해상도 변경 관련 이벤트에 연결할 수 있습니다.
    void Update()
    {
        UpdateCrop();
    }

    public void UpdateCrop()
    {
        // 현재 화면 비율 계산
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = screenAspect / targetAspect;

        // Rect: 카메라가 화면의 어느 부분에 렌더링할지 결정 (x, y, 너비, 높이)
        Rect rect = cam.rect;

        if (scaleHeight < 1.0f) // 현재 화면이 목표보다 세로로 길쭉할 때 (예: 아이폰)
        {
            // 위아래에 검은 띠(레터박스)를 추가
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else // 현재 화면이 목표보다 가로로 넓을 때 (예: Z Fold)
        {
            // 양옆에 검은 띠(필러박스)를 추가
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        cam.rect = rect;
    }
}
