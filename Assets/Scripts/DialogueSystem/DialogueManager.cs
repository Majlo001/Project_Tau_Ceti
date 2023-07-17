 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    private static DialogueManager s_instance;
    // private GameObject[] m_choices;
    // private TextMeshProUGUI[] m_choicesText;

    public bool IsDialoguePlaying { get; private set; }
    public GameObject DialoguePanel;
    public TextMeshProUGUI DialogueText;
    public Story CurrentStory;

    void Awake()
    {
        if (s_instance != null)
        {
            
        }
        s_instance = this;
    } 

    void Start()
    {
        IsDialoguePlaying = false;
        DialoguePanel.SetActive(false);

        // m_choicesText = new TextMeshProUGUI[m_choices.Length];
        // for (int i = 0; i < m_choicesText.Length; i++)
        // {
        //     m_choicesText[i] = m_choices[i].GetComponentInChildren<TextMeshProUGUI>(); 
        // }
    }

    void Update() 
    {
        if (!IsDialoguePlaying) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    public static DialogueManager GetInstance()
    {
        return s_instance;
    } 

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        CurrentStory = new Story(inkJSON.text);
        IsDialoguePlaying = true;
        DialoguePanel.SetActive(true);
        ContinueStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        IsDialoguePlaying = false;
        DialoguePanel.SetActive(false);
        DialogueText.text = "";
    }

    void ContinueStory() 
    {
        Debug.Log("Continued!");
        if (CurrentStory.canContinue)
        {
            DialogueText.text = CurrentStory.Continue();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    // void DisplayChoices()
    // {

    // }
}
