using UnityEngine;
using UnityEngine.UI;

public class UI_SignUp : MonoBehaviour
{
    [SerializeField] private InputField _userNameInput;
    [SerializeField] private InputField _passwordInput;
    [SerializeField] private Button _signUpBtn;

    [SerializeField] private GameObject _loginPanel;

    private void Start()
    {
        _signUpBtn?.onClick.AddListener(OnSignUped);
    }

    private void OnSignUped()
    {
        string username = _userNameInput.text;
        string password = _passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.Log("���̵�� ��й�ȣ�� �Է����ּ���.");
            return;
        }

        SignupManager.Instance.SignUp(username, password);

        gameObject?.SetActive(false);
        _loginPanel?.SetActive(true);
    }
}
