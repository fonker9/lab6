using UnityEngine;
using UnityEngine.UI;
using Core;

namespace UI
{
    /// <summary>
    /// Управляет кнопками действий на столе баристы.
    /// </summary>
    public class BrewingControlView : MonoBehaviour
    {
        [SerializeField] private Button serveButton;
        [SerializeField] private Button clearButton;

        private void Start()
        {
            if (serveButton != null) serveButton.onClick.AddListener(OnServeClicked);
            if (clearButton != null) clearButton.onClick.AddListener(OnClearClicked);
        }

        private void OnServeClicked()
        {
            // Отдаем напиток текущему посетителю
            GameFlowManager.Instance.ServeDrink();
        }

        private void OnClearClicked()
        {
            // Очищаем котел, если напутали с ингредиентами
            InventoryManager.Instance.ClearCauldron();
        }
    }
}
