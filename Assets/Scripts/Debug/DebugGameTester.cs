using UnityEngine;
using UnityEngine.InputSystem;
using Core;
using Data;

public class DebugGameTester : MonoBehaviour
{
    [Header("Тестовые ингредиенты")]
    [SerializeField] private IngredientData ingredientA;
    [SerializeField] private IngredientData ingredientB;

    void Update()
    {
        if (Keyboard.current == null) return;

        // Кнопка 1 — бросить в котел ингредиент А
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            InventoryManager.Instance.AddIngredient(ingredientA);
        }

        // Кнопка 2 — бросить в котел ингредиент В
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            InventoryManager.Instance.AddIngredient(ingredientB);
        }

        // Пробел — продвинуть диалог, если кто-то говорит. 
        // ИЛИ подать напиток, если сейчас фаза варки!
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (DialogueManager.Instance.IsDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else
            {
                // Если никто не говорит, значит мы в фазе варки — отдаем напиток
                GameFlowManager.Instance.ServeDrink();
            }
        }
    }
}