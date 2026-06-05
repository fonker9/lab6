using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Core
{
    /// <summary>
    /// Хранит состояние прогресса: какие рецепты игрок уже открыл.
    /// </summary>
    public class RecipeBookManager : SingletonBase<RecipeBookManager>
    {
        // Список ID рецептов, которые сейчас разблокированы
        private HashSet<string> _unlockedRecipeIDs = new HashSet<string>();

        /// <summary>
        /// Открывает новый рецепт по его ID
        /// </summary>
        public void UnlockRecipe(string recipeID)
        {
            if (string.IsNullOrEmpty(recipeID)) return;

            if (!_unlockedRecipeIDs.Contains(recipeID))
            {
                _unlockedRecipeIDs.Add(recipeID);
                Debug.Log($"<color=lime>[RecipeBook] ОТКРЫТ НОВЫЙ РЕЦЕПТ: {recipeID}</color>");
            }
        }

        /// <summary>
        /// Проверяет, открыт ли рецепт для отображения в книге
        /// </summary>
        public bool IsRecipeUnlocked(string recipeID)
        {
            return _unlockedRecipeIDs.Contains(recipeID);
        }
    }
}