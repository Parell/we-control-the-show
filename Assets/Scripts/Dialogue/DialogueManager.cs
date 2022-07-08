using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] SODialogue dialogues;
    [SerializeField] Text dialogueText;
    public int sentenceIndex;

    public int SentenceIndex { get { return sentenceIndex; } }

    static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }

    public delegate void nextDialogueDelegate();
    public static nextDialogueDelegate NextDialogueDelegate;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        DisplayDialogue(0);
    }

    void DisplayDialogue(int setenceIndex)
    {
        dialogueText.text = dialogues.setences[setenceIndex].setences;

    }

    public void NextDialogue()
    {
        if (sentenceIndex < dialogues.setences.Length - 1)
        {
            sentenceIndex++;
            NextDialogueDelegate?.Invoke();
            DisplayDialogue(sentenceIndex);
        }
    }

    public void BackDialogue()
    {
        if (sentenceIndex > 0)
        {
            sentenceIndex--;
            DisplayDialogue(sentenceIndex);
        }
    }
}
