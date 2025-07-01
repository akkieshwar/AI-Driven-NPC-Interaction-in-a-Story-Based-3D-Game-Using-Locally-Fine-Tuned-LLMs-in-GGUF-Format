using UnityEngine;
using TMPro;

public class ChatboxUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public TMP_Text outputText;
    public LLMChatClient chatClient; // Drag in Unity Inspector

    private void Update()
    {
        // Press Enter to send if NPC is not typing
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!chatClient.isTyping && !string.IsNullOrEmpty(inputField.text))
            {
                SendPlayerMessage();
            }
        }
    }

    public void SendPlayerMessage()
    {
        string playerMessage = inputField.text.Trim();
        if (!string.IsNullOrEmpty(playerMessage))
        {
            // Send message to the LLM chat system
            chatClient.SendMessageToLLM(playerMessage);
            inputField.text = "";
            inputField.interactable = false; // Lock input while NPC is replying
        }
    }

    public void DisplayNPCReply(string reply)
    {
        // This function is unused with typewriter effect
        outputText.text = reply;
        inputField.interactable = true;
    }
}
