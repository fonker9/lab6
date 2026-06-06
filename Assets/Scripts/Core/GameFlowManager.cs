using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameFlowManager : SingletonBase<GameFlowManager>
    {
        public enum GameState { Intro, CustomerDialogue, BrewingPhase, CustomerReaction, DaySummary }

        // ==========================================
        // === СОБЫТИЯ ДЛЯ UI ПЕРСОНАЖЕЙ (НОВОЕ) ===
        // ==========================================
        public event Action<CustomerData> OnCustomerArrived; // Срабатывает, когда заходит новый гость
        public event Action<bool> OnCustomerServed;          // Срабатывает при отдаче зелья (true - успех, false - провал)
        public event Action OnCustomerLeft;                  // Срабатывает, когда гость уходит
        // ==========================================

        [Header("Конфигурация Дня")]
        [SerializeField] private List<CustomerData> dailyCustomers;

        private GameState _currentState;
        private int _currentCustomerIndex = 0;
        private int _currentScore = 0;
        private CustomerData _currentCustomer;

        private void Start()
        {
            InitializeBaseRecipes();
            StartDay();
        }

        private void OnEnable()
        {
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

                // === ДЛЯ UI ПЕРСОНАЖЕЙ ===
                // Говорим визуалу: "Появился новый гость, включи его нейтральный спрайт"
                OnCustomerArrived?.Invoke(_currentCustomer);

                Debug.Log($"\n<color=cyan>[GameFlow] Посетитель [{_currentCustomerIndex + 1}/{dailyCustomers.Count}] заходит в кафе: {_currentCustomer.CustomerName}</color>");

                DialogueManager.Instance.StartDialogue(_currentCustomer.IntroDialogue);
            }
            else
            {
                EndDay();
            }
        }

        private void OnDialogueEndedHandler()
        {
            if (_currentState == GameState.CustomerDialogue)
            {
                _currentState = GameState.BrewingPhase;
                Debug.Log("[GameFlow] Ждем, пока игрок сварит и отдаст напиток... (Нажмите Space в тестере для варки)");
            }
            else if (_currentState == GameState.CustomerReaction)
            {
                // === ДЛЯ UI ПЕРСОНАЖЕЙ ===
                // Перед тем как переключить индекс на нового гостя, говорим старому: "Твой диалог окончен, скройся с экрана"
                OnCustomerLeft?.Invoke();

                _currentCustomerIndex++;
                MoveToNextCustomer();
            }
        }

        public void ServeDrink()
        {
            if (_currentState != GameState.BrewingPhase) return;

            RecipeData brewedPotion = BrewingManager.Instance.BrewPotion();
            _currentState = GameState.CustomerReaction;

            // Проверяем, угадал ли игрок рецепт
            if (brewedPotion != null && brewedPotion.RecipeID == _currentCustomer.DesiredRecipe.RecipeID)
            {
                // УСПЕХ
                _currentScore += 1;

                // === ДЛЯ UI ПЕРСОНАЖЕЙ ===
                // Сигналим визуалу: "Успех! Включи радостную эмоцию"
                OnCustomerServed?.Invoke(true);

                Debug.Log($"<color=green>[GameFlow] Правильно! Очки: {_currentScore}</color>");

                if (_currentCustomer.IsRecipeSecret)
                {
                    RecipeBookManager.Instance.UnlockRecipe(brewedPotion.RecipeID);
                }

                DialogueManager.Instance.StartDialogue(_currentCustomer.SuccessDialogue);
            }
            else
            {
                // ПРОВАЛ
                _currentScore -= 1;

                // === ДЛЯ UI ПЕРСОНАЖЕЙ ===
                // Сигналим визуалу: "Ошибка! Включи грустную эмоцию"
                OnCustomerServed?.Invoke(false);

                Debug.Log($"<color=red>[GameFlow] Не угадал. Очки: {_currentScore}</color>");

                DialogueManager.Instance.StartDialogue(_currentCustomer.FailureDialogue);
            }
        }

        private void EndDay()
        {
            _currentState = GameState.DaySummary;
            Debug.Log("\n<color=orange>[GameFlow] === КОНЕЦ РАБОЧЕГО ДНЯ ===</color>");
            Debug.Log($"Итоговые очки: {_currentScore}");

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
            // Место для будущей инициализации базовых рецептов

            // Разблокируем базовые рецепты для первых двух посетителей
            if (RecipeBookManager.Instance != null)
            {
                RecipeBookManager.Instance.UnlockRecipe("vigorPotion"); // ID твоего первого рецепта
                RecipeBookManager.Instance.UnlockRecipe("luck_elixir");    // ID твоего второго рецепта
            }
        }
    }
}