using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;
using Data;

namespace UI
{
    /// <summary>
    /// Отображает текст диалога и имя спикера на экране с использованием CanvasGroup.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))] // Unity сама добавит компонент, если его нет
    public class DialogueView : MonoBehaviour
    {
        [Header("UI Элементы")]
        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button nextLineButton;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            // На старте скрываем панель, если менеджер еще не запустил диалог
            if (DialogueManager.Instance == null || !DialogueManager.Instance.IsDialogueActive)
            {
                HideDialoguePanel();
            }
        }

        private void OnEnable()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnLineChanged += UpdateDialogueUI;
                DialogueManager.Instance.OnDialogueEnded += HideDialoguePanel;
            }

            if (nextLineButton != null)
            {
                nextLineButton.onClick.AddListener(OnNextLineClicked);
            }
        }

        private void OnDisable()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnLineChanged -= UpdateDialogueUI;
                DialogueManager.Instance.OnDialogueEnded -= HideDialoguePanel;
            }
        }

        private void UpdateDialogueUI(DialogueLine line)
        {
            // Делаем панель видимой и кликабельной
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            speakerNameText.text = line.SpeakerName;
            dialogueText.text = line.Text;
        }

        private void HideDialoguePanel()
        {
            // Скрываем панель визуально и физически для мыши, 
            // но сам GameObject остается ACTIVE = TRUE. Скрипт продолжает слушать ивенты!
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        private void OnNextLineClicked()
        {
            DialogueManager.Instance.AdvanceDialogue();
        }
    }
}
