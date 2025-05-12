using UnityEngine;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour
{
    [SerializeField] private InputField _userNameInput;
    [SerializeField] private InputField _passwordInput;
    [SerializeField] private Button _loginButton;

    private void Start()
    {
        _loginButton?.onClick.AddListener(OnLogined);
    }

    private void OnLogined()
    {
        string username = _userNameInput.text;
        string password = _passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.Log("���̵�� ��й�ȣ�� �Է����ּ���.");
            return;
        }

        LoginManager.Instance.TryLogin(username, password);
    }
}
