using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ChestView : MonoBehaviour
    {
        [Header("UI Панель Ингредиентов")]
        [SerializeField] private CanvasGroup ingredientsPanelGroup;

        private Button _chestButton;
        private bool _isOpen = false;

        private void Awake()
        {
            _chestButton = GetComponent<Button>();
            _chestButton.onClick.AddListener(ToggleChest);

            // На старте сундук закрыт
            CloseChest();
        }

        public void ToggleChest()
        {
            if (_isOpen) CloseChest();
            else OpenChest();
        }

        private void OpenChest()
        {
            _isOpen = true;
            ingredientsPanelGroup.alpha = 1f;
            ingredientsPanelGroup.blocksRaycasts = true;
            ingredientsPanelGroup.interactable = true;
            Debug.Log("[UI] Сундук открыт.");
        }

        private void CloseChest()
        {
            _isOpen = false;
            ingredientsPanelGroup.alpha = 0f;
            ingredientsPanelGroup.blocksRaycasts = false;
            ingredientsPanelGroup.interactable = false;
            Debug.Log("[UI] Сундук закрыт.");
        }
    }
}
