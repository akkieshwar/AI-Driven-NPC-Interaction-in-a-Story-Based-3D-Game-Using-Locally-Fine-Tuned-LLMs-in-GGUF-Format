using TMPro;
using UnityEngine;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public float typeSpeed = 0.02f;
    private TMP_Text textComponent;
    public bool isTyping = false;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    public IEnumerator TypeText(string fullText)  // ✅ Make this public
    {
        isTyping = true;
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }
}
