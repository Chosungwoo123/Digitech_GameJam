using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI talkText;

    public Image bgImage;

    public DialogueInfo[] infos;

    public string nextScene;

    public AudioSource typingSound;
    public AudioClip sound;

    private int dialogueIndex = 0;

    private bool isTypingEnd = true;

    private void Start()
    {
        nameText.text = infos[dialogueIndex].Name;
        StartCoroutine(TypeSentence());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (dialogueIndex < infos.Length - 1 && isTypingEnd)
            {
                dialogueIndex++;

                nameText.text = infos[dialogueIndex].Name;
                StartCoroutine(TypeSentence());
            }
            else if(dialogueIndex >= infos.Length - 1)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    private IEnumerator TypeSentence()
    {
        isTypingEnd = false;

        talkText.text = "";

        foreach (var letter in infos[dialogueIndex].talkText)
        {
            talkText.text += letter;
            typingSound.PlayOneShot(sound);
            yield return new WaitForSeconds(0.05f);
        }

        isTypingEnd = true;
    }
}

[System.Serializable]
public struct DialogueInfo
{
    public string Name;
    public string talkText;
}