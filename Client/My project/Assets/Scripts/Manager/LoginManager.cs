using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{  
    public static LoginManager Instance { get; private set; }

    private string _loginUrl = "http://localhost:5000/api/auth/login";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void TryLogin(string username, string password)
    {
        StartCoroutine(LoginCor(username, password));
    }

    private IEnumerator LoginCor(string username, string password)
    {
        var loginData = new LoginRequest { Username = username, Password = password };
        string jsonData = JsonUtility.ToJson(loginData);

        UnityWebRequest req = new UnityWebRequest(_loginUrl, "POST");
        byte[] body = Encoding.UTF8.GetBytes(jsonData);

        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("로그인 성공: " + req.downloadHandler.text);

            TokenResponse response = JsonUtility.FromJson<TokenResponse>(req.downloadHandler.text);
            PlayerPrefs.SetString("JWT_TOKEN", response.token);
            PlayerPrefs.SetString("USERNAME", response.username);

            Debug.Log("로그인한 유저 이름: " + response.username);
        }
        else
        {
            Debug.LogWarning("로그인 실패: " + req.error);
        }
    }

    [System.Serializable]
    private class LoginRequest
    {
        public string Username;
        public string Password;
    }

    [System.Serializable]
    private class TokenResponse
    {
        public string token;
        public string username;
    }
}
