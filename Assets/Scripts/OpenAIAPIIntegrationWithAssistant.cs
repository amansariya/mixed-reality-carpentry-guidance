using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAIAPIIntegrationWithAssistant : MonoBehaviour
{
    private readonly string openAIAssistantURL = "https://api.openai.com/v1/assistants/asst_gnffa1ucfCZ6n9A5xUxNZkTQ/call";
    private readonly string apiKey = "";

    public void SendRequestToOpenAI(string userInput)
    {
        StartCoroutine(PostRequest(userInput));
    }

    private IEnumerator PostRequest(string userInput)
    {
        var requestBody = new
        {
            // Depending on the API, you might pass user input directly or within a structure
            input = userInput
        };

        string json = JsonUtility.ToJson(requestBody);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(openAIAssistantURL, "POST"))
        {
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                // Process the response
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }
}
