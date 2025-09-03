using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInUI : MonoBehaviour
{
    public GameObject loginPanel;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button signUpButton;
    public Button signInButton;
    public TMP_Text resultText;

    [SerializeField] private PannelController panel;

    void Start()
    {
        signUpButton.onClick.AddListener(OnSignUp);
        signInButton.onClick.AddListener(OnSignIn);

        FirebaseManager.Instance.OnAuthReady += HandleAuthReady;
        FirebaseManager.Instance.OnLoginSuccess += HandleLoginSuccess;
    }

    private void HandleAuthReady()
    {
        if (FirebaseManager.Instance.user == null)
        {
            loginPanel.SetActive(true);
            Debug.Log("로그인한 계정이 없습니다.");
        }
    }

    private void HandleLoginSuccess()
    {
        if(panel.gameObject.activeInHierarchy)
            StartCoroutine(panel.DoPopDownAnimation());
    }

    private void OnSignUp()
    {
        resultText.text = "회원가입중";
        FirebaseManager.Instance.SignUpWithEmail(emailInput.text, passwordInput.text, (isSuccess, message) => {
            resultText.text = message;
        });
    }

    

    private void OnSignIn()
    {
        resultText.text = "로그인중";
        FirebaseManager.Instance.SignInWithEmail(emailInput.text, passwordInput.text, (isSuccess, message) => {
            resultText.text = message;
        });
    }
}
