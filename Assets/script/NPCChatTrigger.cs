using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class NPCChatTrigger : MonoBehaviour
{
    public GameObject talkPromptUI;
    public GameObject chatBoxPanel;
    public Animator npcAnimator;
    public static bool playerInRange = false;

    private void Start()
    {
        if (talkPromptUI != null)
            talkPromptUI.SetActive(false);

        if (chatBoxPanel != null)
            chatBoxPanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenChat();
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseChat();
        }
    }

    private void OpenChat()
    {
        if (npcAnimator != null)
            npcAnimator.SetBool("speak", true);

        if (talkPromptUI != null)
            talkPromptUI.SetActive(false);

        if (chatBoxPanel != null)
        {
            chatBoxPanel.SetActive(true);
            PlayerMovement.isChatBoxOpen = true;

            // Focus input field
            TMP_InputField inputField = chatBoxPanel.GetComponentInChildren<TMP_InputField>();
            if (inputField != null)
            {
                EventSystem.current.SetSelectedGameObject(inputField.gameObject);
                inputField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }

    private void CloseChat()
    {
        if (npcAnimator != null)
            npcAnimator.SetBool("speak", false);

        if (chatBoxPanel != null)
            chatBoxPanel.SetActive(false);

        // ✅ Show talk prompt only if still inside trigger
        if (playerInRange && talkPromptUI != null)
            talkPromptUI.SetActive(true);
        else if (!playerInRange && talkPromptUI != null)
            talkPromptUI.SetActive(false);

        PlayerMovement.isChatBoxOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (talkPromptUI != null)
                talkPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (talkPromptUI != null)
                talkPromptUI.SetActive(false);

            CloseChat(); // ✅ safely closes chat + resets animation
        }
    }
}
