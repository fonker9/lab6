using UnityEngine;
using UnityEngine.UI;
using Core;
using Data;

namespace UI
{
    public class RecipeBookView : MonoBehaviour
    {
        [Header("������ ����������")]
        [SerializeField] private Button toggleBookButton; // ������������ ������ ��� ����/����

        [Header("UI ������")]
        [SerializeField] private CanvasGroup bookPanelGroup;
        [SerializeField] private Transform slotsContainer;

        [Header("�������")]
        [SerializeField] private GameObject recipeSlotPrefab;

        private bool _isOpen = false; // ������ ������� ��������� �����

        private void Start()
        {
            if (toggleBookButton != null)
                toggleBookButton.onClick.AddListener(ToggleBook);

            // �� ������ ���� ������ ���������� � �������� ������
            CloseBook();
        }

        /// <summary>
        /// �����-�������������. ��� ������, ������� ����� ��� �������.
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
            Debug.Log("[UI] ����� �������� �������.");
        }

        // Было private, стало public, чтобы Unity UI видел этот метод
        public void CloseBook()
        {
            _isOpen = false;
            bookPanelGroup.alpha = 0f;
            bookPanelGroup.blocksRaycasts = false;
            bookPanelGroup.interactable = false;
            Debug.Log("[UI] Книга рецептов ЗАКРЫТА.");
        }

        private void RefreshRecipeList()
        {
            // ������� ������ �����
            foreach (Transform child in slotsContainer)
            {
                Destroy(child.gameObject);
            }

            // ������� ���������� �����
            if (BrewingManager.Instance != null && recipeSlotPrefab != null)
            {
                foreach (RecipeData recipe in BrewingManager.Instance.AllRecipes)
                {
                    GameObject newSlot = Instantiate(recipeSlotPrefab, slotsContainer);
                    newSlot.transform.localScale = Vector3.one; // ���� ���������� �������

                    // === �������� ���� �������� ����� ===
                    // ����� UI-��������� ������������� � ���������� ��� ��������� ���������� � ����
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
