using UnityEngine;

namespace Data
{
    /// <summary>
    /// Класс отвечает за хранение статичных данных об отдельном ингредиенте.
    /// Зачем нужен: Это неделимая единица (атом) нашей системы варки. 
    /// Позволяет геймдизайнеру создавать новые ингредиенты прямо в инспекторе Unity 
    /// без написания нового кода. Данные здесь неизменяемы (Read-Only) во время игры.
    /// </summary>
    [CreateAssetMenu(fileName = "NewIngredient", menuName = "WitchCafe/Ingredient")]
    public class IngredientData : ScriptableObject
    {
        // Идентификатор для логики (по нему будем сравнивать ингредиенты в коде)
        [SerializeField] private string ingredientID;
        
        // Красивое отображаемое название для интерфейса игры (Книги рецептов, диалогов)
        [SerializeField] private string ingredientName;
        
        // Иконка ингредиента, которая будет отображаться в Сундуке или Книге рецептов
        [SerializeField] private Sprite icon;

        // Публичные свойства (Properties) в формате Read-Only для безопасного доступа из других систем.
        // Изменить эти данные из других скриптов нельзя — только прочитать.
        public string IngredientID => ingredientID;
        public string IngredientName => ingredientName;
        public Sprite Icon => icon;
    }
}