using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Core
{
    /// <summary>
    /// Менеджер текущего инвентаря (выбранных для варки ингредиентов).
    /// Зачем нужен: Хранит в себе список того, что игрок кликнул в сундуке,
    /// и позволяет очистить котел или передать содержимое в систему варки.
    /// </summary>
    public class InventoryManager : SingletonBase<InventoryManager>
    {
        // Список ингредиентов, которые сейчас находятся в котле
        private List<IngredientData> _currentIngredients = new List<IngredientData>();

        /// <summary>
        /// Добавить ингредиент в котел (например, при клике по нему в UI сундука)
        /// </summary>
        public void AddIngredient(IngredientData ingredient)
        {
            if (ingredient == null) return;
            
            _currentIngredients.Add(ingredient);
            Debug.Log($"[Inventory] Добавлен ингредиент: {ingredient.IngredientName}. Всего в котле: {_currentIngredients.Count}");
        }

        /// <summary>
        /// Полностью очистить котел (если игрок нажал "Сбросить" или варка завершена)
        /// </summary>
        public void ClearCauldron()
        {
            _currentIngredients.Clear();
            Debug.Log("[Inventory] Котел успешно очищен.");
        }

        /// <summary>
        /// Возвращает копию списка текущих ингредиентов (для проверки рецепта)
        /// </summary>
        public List<IngredientData> GetCurrentIngredients()
        {
            return new List<IngredientData>(_currentIngredients);
        }
    }
}