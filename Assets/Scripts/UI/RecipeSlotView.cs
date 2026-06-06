using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Data;
using Core;

namespace UI
{
    public class RecipeSlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text recipeNameText;
        [SerializeField] private Image recipeIconImage;

        /// <summary>
        /// Настройка визуала слота на основе данных рецепта и его стейта разблокировки
        /// </summary>
        public void Setup(RecipeData data)
        {
            if (data == null) return;

            // Спрашиваем у менеджера книги, открыт ли этот рецепт игроком
            bool isUnlocked = RecipeBookManager.Instance.IsRecipeUnlocked(data.RecipeID);

            if (isUnlocked)
            {
                recipeNameText.text = data.RecipeName;
                if (recipeIconImage != null && data.ResultPotionSprite != null)
                {
                    recipeIconImage.sprite = data.ResultPotionSprite;
                    recipeIconImage.gameObject.SetActive(true);
                }
            }
            else
            {
                recipeNameText.text = "(Неизвестный рецепт)";
                if (recipeIconImage != null) recipeIconImage.gameObject.SetActive(false);
            }
        }
    }
}
