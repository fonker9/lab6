using UnityEngine;
using UnityEngine.UI;
using Core;
using Data;

namespace UI
{
    public class RecipeBookView : MonoBehaviour
    {
        [Header("Кнопка управления")]
        [SerializeField] private Button toggleBookButton; // Единственная кнопка для откр/закр

        [Header("UI Панели")]
        [SerializeField] private CanvasGroup bookPanelGroup;
        [SerializeField] private Transform slotsContainer;

        [Header("Префабы")]
        [SerializeField] private GameObject recipeSlotPrefab;

        private bool _isOpen = false; // Хранит текущее состояние книги

        private void Start()
        {
            if (toggleBookButton != null)
                toggleBookButton.onClick.AddListener(ToggleBook);

            // На старте игра всегда начинается с закрытой книгой
            CloseBook();
        }

        /// <summary>
        /// Метод-переключатель. Сам решает, открыть книгу или закрыть.
        /// </summary>
        private void ToggleBook()
        {
            if (_isOpen)
            {
                CloseBook();
            }
            else
            {
                OpenBook();
            }
        }

        private void OpenBook()
        {
            _isOpen = true;
            bookPanelGroup.alpha = 1f;
            bookPanelGroup.blocksRaycasts = true;
            bookPanelGroup.interactable = true;

            RefreshRecipeList();
            Debug.Log("[UI] Книга рецептов ОТКРЫТА.");
        }

        private void CloseBook()
        {
            _isOpen = false;
            bookPanelGroup.alpha = 0f;
            bookPanelGroup.blocksRaycasts = false;
            bookPanelGroup.interactable = false;
            Debug.Log("[UI] Книга рецептов ЗАКРЫТА.");
        }

        private void RefreshRecipeList()
        {
            // Очищаем старые слоты
            foreach (Transform child in slotsContainer)
            {
                Destroy(child.gameObject);
            }

            // Спавним актуальные слоты
            if (BrewingManager.Instance != null && recipeSlotPrefab != null)
            {
                foreach (RecipeData recipe in BrewingManager.Instance.AllRecipes)
                {
                    GameObject newSlot = Instantiate(recipeSlotPrefab, slotsContainer);
                    newSlot.transform.localScale = Vector3.one; // Фикс гигантских шрифтов

                    // === ЖЕЛЕЗНЫЙ ФИКС СМЕЩЕНИЯ ВЛЕВО ===
                    // Берем UI-компонент трансформации и сбрасываем его локальные координаты в ноль
                    RectTransform rectTransform = newSlot.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = Vector2.zero;
                        rectTransform.localPosition = Vector3.zero;
                    }
                    // ====================================

                    RecipeSlotView slotView = newSlot.GetComponent<RecipeSlotView>();
                    if (slotView != null)
                    {
                        slotView.Setup(recipe);
                    }
                }
            }
        }
    }
}
