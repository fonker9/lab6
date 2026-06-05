using UnityEngine;
using UnityEngine.InputSystem; // Добавляем новый неймсейс
using Core;
using Data;

public class DebugBrewTester : MonoBehaviour
{
    [Header("Тестовые ингредиенты")]
    [SerializeField] private IngredientData ingredientA;
    [SerializeField] private IngredientData ingredientB;

    void Update()
    {
        // Проверяем, подключена ли клавиатура вообще (защита от ошибок)
        if (Keyboard.current == null) return;

        // Цифра 1 на основной клавиатуре
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            InventoryManager.Instance.AddIngredient(ingredientA);
        }

        // Цифра 2 на основной клавиатуре
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            InventoryManager.Instance.AddIngredient(ingredientB);
        }

        // Клавиша Пробел
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            BrewingManager.Instance.BrewPotion();
        }
    }
}