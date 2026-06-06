using UnityEngine;
using UnityEngine.UI;
using Core;
using Data;

namespace UI
{
    /// <summary>
    /// Отображает аватар текущего посетителя и меняет его эмоции.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class CustomerView : MonoBehaviour
    {
        [Header("UI Компоненты")]
        [SerializeField] private Image customerAvatarImage; // Ссылка на UI Image, где рендерится персонаж

        private CanvasGroup _canvasGroup;
        private CustomerData _activeCustomerData;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            HideCustomer();
        }

        private void OnEnable()
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnCustomerArrived += HandleCustomerArrived;
                GameFlowManager.Instance.OnCustomerServed += HandleCustomerServed;
                GameFlowManager.Instance.OnCustomerLeft += HandleCustomerLeft;
            }
        }

        private void OnDisable()
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnCustomerArrived -= HandleCustomerArrived;
                GameFlowManager.Instance.OnCustomerServed -= HandleCustomerServed;
                GameFlowManager.Instance.OnCustomerLeft -= HandleCustomerLeft;
            }
        }

        // 1. Пришел новый гость — включаем визуал и ставим НЕЙТРАЛЬНЫЙ спрайт
        private void HandleCustomerArrived(CustomerData customer)
        {
            if (customer == null || customerAvatarImage == null) return;

            _activeCustomerData = customer;
            customerAvatarImage.sprite = customer.NeutralSprite;

            // Проявляем картинку на Canvas
            _canvasGroup.alpha = 1f;
        }

        // 2. Игрок отдал зелье — меняем спрайт на Радость или Грусть. 
        // Персонаж остаётся на экране, пока крутится финальный диалог!
        private void HandleCustomerServed(bool isSuccess)
        {
            if (_activeCustomerData == null || customerAvatarImage == null) return;

            if (isSuccess)
            {
                customerAvatarImage.sprite = _activeCustomerData.JoyfulSprite;
                Debug.Log($"[UI Visual] {_activeCustomerData.CustomerName} радуется!");
            }
            else
            {
                customerAvatarImage.sprite = _activeCustomerData.SadSprite;
                Debug.Log($"[UI Visual] {_activeCustomerData.CustomerName} расстроен...");
            }
        }

        // 3. Гость ушел — прячем картинку до следующего посетителя
        private void HandleCustomerLeft()
        {
            HideCustomer();
        }

        private void HideCustomer()
        {
            _canvasGroup.alpha = 0f;
            _activeCustomerData = null;
        }
    }
}
