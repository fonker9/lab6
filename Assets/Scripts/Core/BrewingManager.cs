using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Core
{
    /// <summary>
    /// Менеджер варки напитков.
    /// Зачем нужен: Хранит базу всех существующих рецептов игры и сопоставляет
    /// текущее содержимое котла с этой базой.
    /// </summary>
    public class BrewingManager : SingletonBase<BrewingManager>
    {
        [Tooltip("Список всех доступных рецептов в игре (заполняется в Инспекторе)")]
        [SerializeField] private List<RecipeData> allRecipes;
        public List<RecipeData> AllRecipes => allRecipes;

        /// <summary>
        /// Запускает процесс варки на основе того, что сейчас лежит в InventoryManager
        /// </summary>
        /// <returns>Возвращает RecipeData, если рецепт найден. Возвращает null, если получилась жижа.</returns>
        public RecipeData BrewPotion()
        {
            // Получаем то, что игрок набросал в котел
            List<IngredientData> cauldronIngredients = InventoryManager.Instance.GetCurrentIngredients();

            // Ищем подходящий рецепт в нашей базе данных
            foreach (RecipeData recipe in allRecipes)
            {
                if (AreIngredientsMatching(recipe.RequiredIngredients, cauldronIngredients))
                {
                    Debug.Log($"[Brewing] Успех! Сварено: {recipe.RecipeName}");
                    InventoryManager.Instance.ClearCauldron(); // Очищаем котел после успешной варки
                    return recipe;
                }
            }

            Debug.LogWarning("[Brewing] Провал... Получилась неопознанная бурлящая жижа.");
            InventoryManager.Instance.ClearCauldron(); // Очищаем котел даже при ошибке
            return null;
        }

        /// <summary>
        /// Алгоритм сравнения двух списков ингредиентов без учета их порядка.
        /// Учитывает дубликаты (например, если в рецепте нужно 2 корня мандрагоры).
        /// </summary>
        private bool AreIngredientsMatching(List<IngredientData> required, List<IngredientData> input)
        {
            // Если количество не совпадает, то это точно не тот рецепт
            if (required.Count != input.Count) return false;

            // Создаем временную копию ввода, чтобы не портить оригинальные данные
            List<IngredientData> inputCopy = new List<IngredientData>(input);

            foreach (IngredientData reqIngredient in required)
            {
                // Ищем нужный ингредиент в копии ввода по ID
                IngredientData found = inputCopy.Find(x => x.IngredientID == reqIngredient.IngredientID);
                
                if (found == null)
                {
                    // Если хотя бы одного нужного ингредиента нет — рецепт не подходит
                    return false;
                }

                // Удаляем найденный элемент из копии, чтобы правильно обрабатывать одинаковые ингредиенты
                inputCopy.Remove(found);
            }

            return true;
        }
    }
}