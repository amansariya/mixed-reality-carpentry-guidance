using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Models;
using OpenAI_API.Chat;

public class ChatGPTSample : MonoBehaviour
{
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    [SerializeField]
    string APIString;

    void Start()
    {
        api = new OpenAIAPI(APIString);
        StartConversation();
        GetResponse();
    }


    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "I will give you 5 float values. All you need to do is return the difference between each value and 5.0. Do not write any other text, I just need 5 values as output.")
        };
    }

    private async void GetResponse()
    {
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.TextContent = "2.0 6.25 4.95 8.0 10.5";

        Debug.Log(string.Format("{0}: {1}", userMessage.Role, userMessage.TextContent));

        messages.Add(userMessage);

        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo_1106,
            Temperature = 0,
            MaxTokens = 50,
            Messages = messages
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.TextContent = chatResult.Choices[0].Message.TextContent;
        Debug.Log(string.Format("{0}: {1}", responseMessage.Role, responseMessage.TextContent));

        messages.Add(responseMessage);
    }

}
