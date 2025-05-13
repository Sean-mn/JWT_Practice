using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class SignUpRequest
{
    public string username;
    public string password;
}

public class SignupManager : MonoBehaviour
{
    public static SignupManager Instance { get; private set; }

    private string _serverUrl = "http://localhost:5000/api/auth";
    public bool IsSignUped { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void SignUp(string username, string password)
    {
        SignUpRequest request = new SignUpRequest
        {
            password = password,
            username = username,    
        };

        string jsonData = JsonUtility.ToJson(request);
        StartCoroutine(PostSignUp(jsonData));
    }

    private IEnumerator PostSignUp(string json)
    {
        using UnityWebRequest request = new UnityWebRequest($"{_serverUrl}/signup", "POST");
        byte[] body = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("회원가입 성공!");
        }
        else
        {
            Debug.LogWarning($"회원가입 실패: {request.error}");
        }
    }
}
