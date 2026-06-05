using UnityEngine;
using UnityEngine.InputSystem;
using Core;
using Data;

public class DebugCustomerTester : MonoBehaviour
{
    [Header("Тестируемый посетитель")]
    [SerializeField] private CustomerData customer;

    private void OnEnable()
    {
        // Подписываемся на событие окончания диалога (тест обратной связи)
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnded += OnDialogueFinishedReaction;
        }
    }

    private void OnDisable()
    {
        // Обязательно отписываемся при уничтожении объекта, чтобы избежать утечек памяти
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueEnded -= OnDialogueFinishedReaction;
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Нажми 3, чтобы запустить диалог посетителя через DialogueManager
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            if (customer != null && customer.IntroDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(customer.IntroDialogue);
            }
            else
            {
                Debug.LogError("[Test] Проверь, назначен ли посетитель и его Intro Dialogue!");
            }
        }

        // Нажми Пробел, чтобы продвинуть диалог вперед, ЕСЛИ он сейчас активен
        if (Keyboard.current.spaceKey.wasPressedThisFrame && DialogueManager.Instance.IsDialogueActive)
        {
            DialogueManager.Instance.AdvanceDialogue();
        }
    }

    // Этот метод сработает автоматически, когда DialogueManager закончит работу
    private void OnDialogueFinishedReaction()
    {
        Debug.Log("<color=green>[Test System] Сигнал получен! Диалог завершен. Теперь можно варить заказ.</color>");
    }
}