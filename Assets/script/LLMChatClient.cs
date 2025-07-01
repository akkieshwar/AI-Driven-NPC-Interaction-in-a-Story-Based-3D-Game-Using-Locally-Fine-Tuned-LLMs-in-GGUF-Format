using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class LLMChatClient : MonoBehaviour
{
    [Header("LLM Settings")]
    public string llmServerUrl = "http://127.0.0.1:5000/chat"; // Flask server URL

    [Header("UI References")]
    public TMP_Text npcReplyText; // Output text UI for NPC reply

    private TypewriterEffect typewriter;
    public bool isTyping = false;  // Used to block input while typing

    private void Awake()
    {
        // Get reference to TypewriterEffect component on the same TextMeshPro
        typewriter = npcReplyText.GetComponent<TypewriterEffect>();
        if (typewriter == null)
        {
            Debug.LogError("Missing TypewriterEffect on NPC Reply Text!");
        }
    }

    public void SendMessageToLLM(string playerMessage)
    {
        if (!string.IsNullOrEmpty(playerMessage) && !isTyping)
        {
            StartCoroutine(SendChatRequest(playerMessage));
        }
        else
        {
            Debug.LogWarning("⏳ Still waiting or message is empty.");
        }
    }

    private IEnumerator SendChatRequest(string message)
    {
        string jsonPayload = "{\"message\":\"" + message.Replace("\"", "\\\"") + "\"}";

        UnityWebRequest request = new UnityWebRequest(llmServerUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Received response from LLM server!");
            string jsonResponse = request.downloadHandler.text;
            string npcReply = ExtractReplyFromJson(jsonResponse);

            if (npcReplyText != null && typewriter != null)
            {
                isTyping = true;

                // Start typewriter effect and wait for it to finish
                yield return StartCoroutine(typewriter.TypeText(npcReply));

                isTyping = false;

                // Re-enable input field after NPC is done replying
                ChatboxUI chatbox = FindObjectOfType<ChatboxUI>();
                if (chatbox != null && chatbox.inputField != null)
                {
                    chatbox.inputField.interactable = true;
                }
            }
        }
        else
        {
            Debug.LogError("❌ LLM Server Error: " + request.error);
            if (npcReplyText != null)
                npcReplyText.text = "Sorry, I can't talk right now.";
        }
    }

    private string ExtractReplyFromJson(string json)
    {
        const string key = "\"content\":\"";
        int startIndex = json.IndexOf(key);
        if (startIndex >= 0)
        {
            startIndex += key.Length;
            int endIndex = json.IndexOf("\"", startIndex);
            if (endIndex > startIndex)
                return json.Substring(startIndex, endIndex - startIndex);
        }
        return "No reply.";
    }
}
