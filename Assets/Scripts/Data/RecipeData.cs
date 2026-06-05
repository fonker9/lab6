using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Класс отвечает за хранение информации о конкретном рецепте напитка.
    /// Зачем нужен: Служит эталоном (шаблоном) для системы варки. 
    /// BrewingManager будет сверять список того, что игрок бросил в котел, 
    /// со списком 'requiredIngredients' из этого класса, чтобы понять, какой напиток получился.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "WitchCafe/Recipe")]
    public class RecipeData : ScriptableObject
    {
        // Уникальный ID рецепта (понадобится для сохранения прогресса: открыт/закрыт)
        [SerializeField] private string recipeID;
        
        // Название готового напитка (например, "Зелье бодрости")
        [SerializeField] private string recipeName;
        
        // Список ингредиентов, которые игроку необходимо собрать для успешного приготовления
        [SerializeField] private List<IngredientData> requiredIngredients;
        
        // Спрайт готового напитка, который появится на экране после успешной варки
        [SerializeField] private Sprite resultPotionSprite;

        // Публичный доступ на чтение (инкапсуляция данных)
        public string RecipeID => recipeID;
        public string RecipeName => recipeName;
        public List<IngredientData> RequiredIngredients => requiredIngredients;
        public Sprite ResultPotionSprite => resultPotionSprite;
    }
}