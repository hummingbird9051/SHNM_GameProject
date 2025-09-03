using UnityEngine;


[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    // ��ǥ ȭ�� ���� (16:9)
    public float targetAspect = 16.0f / 9.0f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateCrop();
    }

    // ȭ�� ũ�Ⱑ ����� ������ ȣ��� �� �ֵ��� Update�� �ְų�,
    // �ػ� ���� ���� �̺�Ʈ�� ������ �� �ֽ��ϴ�.
    void Update()
    {
        UpdateCrop();
    }

    public void UpdateCrop()
    {
        // ���� ȭ�� ���� ���
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = screenAspect / targetAspect;

        // Rect: ī�޶� ȭ���� ��� �κп� ���������� ���� (x, y, �ʺ�, ����)
        Rect rect = cam.rect;

        if (scaleHeight < 1.0f) // ���� ȭ���� ��ǥ���� ���η� ������ �� (��: ������)
        {
            // ���Ʒ��� ���� ��(���͹ڽ�)�� �߰�
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else // ���� ȭ���� ��ǥ���� ���η� ���� �� (��: Z Fold)
        {
            // �翷�� ���� ��(�ʷ��ڽ�)�� �߰�
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        cam.rect = rect;
    }
}
