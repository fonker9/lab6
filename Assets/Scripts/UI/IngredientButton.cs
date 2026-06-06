using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core;
using Data;

namespace UI
{
    /// <summary>
    /// Скрипт для кнопки ингредиента. 
    /// Использует SpriteRenderer для отображения визуала в 2D-сцене.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class IngredientButton : MonoBehaviour
    {
        [Header("Данные Ингредиента")]
        [SerializeField] private IngredientData ingredientData;

        [Header("Ссылки на визуал кнопки")]
        [SerializeField] private SpriteRenderer iconRenderer; // Теперь здесь SpriteRenderer вместо Image
        [SerializeField] private TMP_Text nameText;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnIngredientClicked);

            // Инициализируем стартовые данные, если они привязаны в инспекторе
            Init(ingredientData);
        }

        /// <summary>
        /// Динамическая настройка ингредиента под конкретный ассет данных
        /// </summary>
        public void Init(IngredientData data)
        {
            if (data == null) return;

            ingredientData = data;

            // Напрямую передаем спрайт в компонент SpriteRenderer
            if (iconRenderer != null)
                iconRenderer.sprite = data.Icon;

            if (nameText != null)
                nameText.text = data.IngredientName;
        }

        private void OnIngredientClicked()
        {
            if (ingredientData == null) return;

            // Бросаем ингредиент в котел
            InventoryManager.Instance.AddIngredient(ingredientData);
        }
    }
}
