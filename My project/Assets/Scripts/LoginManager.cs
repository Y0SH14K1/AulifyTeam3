using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    [SerializeField] 
    TMP_InputField user_input_field;

    [SerializeField] 
    TMP_InputField password_input_field;

    [SerializeField]
    string loginEndpoint;

    [System.Serializable]
    public class Credentials
    {
        public string user, pass;
    }

    [System.Serializable]
    public class Response
    {
        public int code;
        public string message;
    }

    IEnumerator Validate(string user, string pass)
    {
        UnityWebRequest request = UnityWebRequest.PostWwwForm(loginEndpoint, "");
        request.SetRequestHeader("Content-Type", "application/json");

        Credentials credentials = new Credentials();
        credentials.user = user;
        credentials.pass = pass;
        string jsonData = JsonUtility.ToJson(credentials);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData.ToString());
        Debug.Log("Enviando" + jsonData);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("La solicitud se realizó con éxito:" + request.downloadHandler.text);
            Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            Debug.Log("Success: " + response.code);
            Debug.Log("Message: " + response.message);
            if (response.code == 1)
            {
                PlayerPrefs.SetString("token", response.message);
            }
        }
        else
        {
            Debug.Log("La solicitud no se pudo realizar. Error code:" + request.responseCode);
        }
    }

    public void DoLogin()
    {
        // Usar el endpoint del módulo web para autenticación
        string user = user_input_field.text;
        string password = password_input_field.text;
        StartCoroutine(Validate(user, password));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
