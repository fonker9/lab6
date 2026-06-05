using System;
using UnityEngine;
using Data;

namespace Core
{
    /// <summary>
    /// Менеджер диалогов. 
    /// Зачем нужен: Пошагово прокручивает реплики текущего диалога,
    /// оповещает UI об изменении текста и сигнализирует системе GameFlow об окончании разговора.
    /// </summary>
    public class DialogueManager : SingletonBase<DialogueManager>
    {
        // Событие для UI: передает текущую строчку, чтобы обновить текст на экране
        public event Action<DialogueLine> OnLineChanged;
        
        // Событие для GameFlow: сообщает, что посетитель договорил
        public event Action OnDialogueEnded;

        private DialogueData _currentDialogue;
        private int _currentLineIndex;
        private bool _isDialogueActive;

        // Свойство, чтобы другие системы знали, занят ли сейчас экран разговором
        public bool IsDialogueActive => _isDialogueActive;

        /// <summary>
        /// Запустить цепочку диалога
        /// </summary>
        public void StartDialogue(DialogueData dialogue)
        {
            if (dialogue == null || dialogue.Lines.Length == 0)
            {
                Debug.LogWarning("[Dialogue] Попытка запустить пустой или отсутствующий диалог.");
                EndDialogue();
                return;
            }

            _currentDialogue = dialogue;
            _currentLineIndex = 0;
            _isDialogueActive = true;

            Debug.Log("<color=yellow>[Dialogue] === НАЧАЛО РАЗГОВОРА ===</color>");
            DisplayCurrentLine();
        }

        /// <summary>
        /// Перейти к следующей реплике (вызывается по клику мыши или Space)
        /// </summary>
        public void AdvanceDialogue()
        {
            if (!_isDialogueActive) return;

            _currentLineIndex++;

            // Если реплики еще остались — показываем следующую
            if (_currentLineIndex < _currentDialogue.Lines.Length)
            {
                DisplayCurrentLine();
            }
            // Если реплики кончились — закрываем диалог
            else
            {
                EndDialogue();
            }
        }

        private void DisplayCurrentLine()
        {
            DialogueLine currentLine = _currentDialogue.Lines[_currentLineIndex];
            
            // Вывод в консоль для нашего тестирования
            Debug.Log($"[Dialogue] [{_currentLineIndex + 1}/{_currentDialogue.Lines.Length}] " +
                      $"<b>{currentLine.SpeakerName}:</b> \"{currentLine.Text}\"");
            
            // Твой напарник-интерфейсник в будущем просто подпишется на это событие,
            // чтобы его UI-скрипт автоматически обновлял текст на экране.
            OnLineChanged?.Invoke(currentLine);
        }

        private void EndDialogue()
        {
            _isDialogueActive = false;
            _currentDialogue = null;
            
            Debug.Log("<color=yellow>[Dialogue] === КОНЕЦ РАЗГОВОРА ===</color>");
            
            // Оповещаем внешние системы (например, GameFlowManager)
            OnDialogueEnded?.Invoke();
        }
    }
}