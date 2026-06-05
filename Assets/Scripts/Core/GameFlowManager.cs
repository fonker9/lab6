using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Core
{
    public class GameFlowManager : SingletonBase<GameFlowManager>
    {
        public enum GameState { Intro, CustomerDialogue, BrewingPhase, CustomerReaction, DaySummary }

        [Header("Конфигурация Дня")]
        [SerializeField] private List<CustomerData> dailyCustomers;

        private GameState _currentState;
        private int _currentCustomerIndex = 0;
        private int _currentScore = 0;
        private CustomerData _currentCustomer;

        private void Start()
        {
            // На старте MVP разблокируем базовые рецепты, кроме секретного
            // (В будущем это будет загружаться из сохранений)
            InitializeBaseRecipes();
            
            StartDay();
        }

        private void OnEnable()
        {
            // Подписываемся на окончание диалогов
            DialogueManager.Instance.OnDialogueEnded += OnDialogueEndedHandler;
        }

        private void OnDisable()
        {
            if (DialogueManager.Instance != null)
                DialogueManager.Instance.OnDialogueEnded -= OnDialogueEndedHandler;
        }

        private void StartDay()
        {
            Debug.Log("<color=orange>[GameFlow] Обучающий день начался!</color>");
            _currentCustomerIndex = 0;
            _currentScore = 0;
            
            MoveToNextCustomer();
        }

        private void MoveToNextCustomer()
        {
            if (_currentCustomerIndex < dailyCustomers.Count)
            {
                _currentCustomer = dailyCustomers[_currentCustomerIndex];
                _currentState = GameState.CustomerDialogue;
                
                Debug.Log($"\n<color=cyan>[GameFlow] Посетитель [{_currentCustomerIndex + 1}/{dailyCustomers.Count}] заходит в кафе: {_currentCustomer.CustomerName}</color>");
                
                // Запускаем вводный диалог гостя
                DialogueManager.Instance.StartDialogue(_currentCustomer.IntroDialogue);
            }
            else
            {
                EndDay();
            }
        }

        // Срабатывает автоматически, когда DialogueManager закончил крутить текст
        private void OnDialogueEndedHandler()
        {
            // Если гость закончил вводную фразу -> переходим к варке
            if (_currentState == GameState.CustomerDialogue)
            {
                _currentState = GameState.BrewingPhase;
                Debug.Log("[GameFlow] Ждем, пока игрок сварит и отдаст напиток... (Нажмите Space в тестере для варки)");
            }
            // Если гость закончил говорить финальную фразу (успех/провал) -> зовем следующего
            else if (_currentState == GameState.CustomerReaction)
            {
                _currentCustomerIndex++;
                MoveToNextCustomer();
            }
        }

        /// <summary>
        /// Публичный метод, который будет вызываться по кнопке UI "Подать напиток".
        /// Сейчас мы вызовем его из нашего обновленного тестера.
        /// </summary>
        public void ServeDrink()
        {
            if (_currentState != GameState.BrewingPhase) return;

            // Запускаем варку и получаем результат
            RecipeData brewedPotion = BrewingManager.Instance.BrewPotion();

            _currentState = GameState.CustomerReaction;

            // Проверяем, угадал ли игрок рецепт
            if (brewedPotion != null && brewedPotion.RecipeID == _currentCustomer.DesiredRecipe.RecipeID)
            {
                // УСПЕХ
                _currentScore += 1;
                Debug.Log($"<color=green>[GameFlow] Правильно! Очки: {_currentScore}</color>");
                
                // Специфика MVP: если рецепт был секретным (3-й гость) — открываем его в книге!
                if (_currentCustomer.IsRecipeSecret)
                {
                    RecipeBookManager.Instance.UnlockRecipe(brewedPotion.RecipeID);
                }

                // Запускаем радостный диалог гостя
                DialogueManager.Instance.StartDialogue(_currentCustomer.SuccessDialogue);
            }
            else
            {
                // ПРОВАЛ (сварил не то или получилась жижа)
                _currentScore -= 1;
                Debug.Log($"<color=red>[GameFlow] Не угадал. Очки: {_currentScore}</color>");
                
                // Запускаем недовольный диалог гостя
                DialogueManager.Instance.StartDialogue(_currentCustomer.FailureDialogue);
            }
        }

        private void EndDay()
        {
            _currentState = GameState.DaySummary;
            Debug.Log("\n<color=orange>[GameFlow] === КОНЕЦ РАБОЧЕГО ДНЯ ===</color>");
            Debug.Log($"Итоговые очки: {_currentScore}");

            // Оценка по ТЗ
            if (_currentScore >= 3)
            {
                Debug.Log("<color=magenta>[GameFlow] Итог: \"Поздравляю, вы успешно прошли обучение\"</color>");
            }
            else
            {
                Debug.Log("<color=magenta>[GameFlow] Итог: \"Вы можете лучше! Мастерство придёт с опытом\"</color>");
            }
        }

        private void InitializeBaseRecipes()
        {
            // Для теста MVP автоматически откроем какой-нибудь стартовый рецепт
            // Чтобы игрок знал, из чего варить первым двум гостям
            // Например, если у тебя есть ID "energy_potion", можно вписать его сюда:
            // RecipeBookManager.Instance.UnlockRecipe("energy_potion");
        }
    }
}