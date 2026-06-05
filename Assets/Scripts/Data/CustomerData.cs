using UnityEngine;

namespace Data
{
    /// <summary>
    /// Класс хранит все данные о конкретном посетителе, его заказах и репликах.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCustomer", menuName = "WitchCafe/Customer")]
    public class CustomerData : ScriptableObject
    {
        [Header("Визуал и Имя")]
        [SerializeField] private string customerName;
        [SerializeField] private Sprite characterSprite;

        [Header("Заказ")]
        [Tooltip("Какое зелье ожидает этот посетитель")]
        [SerializeField] private RecipeData desiredRecipe;
        
        [Tooltip("Если true, то рецепт изначально закрыт в книге, и игрок должен его угадать")]
        [SerializeField] private bool isRecipeSecret;

        [Header("Сюжетные Диалоги")]
        [Tooltip("Разговор при входе (где озвучивается заказ или намек на него)")]
        [SerializeField] private DialogueData introDialogue;
        
        [Tooltip("Реакция, если игрок отдал правильный напиток")]
        [SerializeField] private DialogueData successDialogue;
        
        [Tooltip("Реакция, если игрок ошибся с рецептом или сварил жижу")]
        [SerializeField] private DialogueData failureDialogue;

        // Публичный доступ на чтение
        public string CustomerName => customerName;
        public Sprite CharacterSprite => characterSprite;
        public RecipeData DesiredRecipe => desiredRecipe;
        public bool IsRecipeSecret => isRecipeSecret;
        public DialogueData IntroDialogue => introDialogue;
        public DialogueData SuccessDialogue => successDialogue;
        public DialogueData FailureDialogue => failureDialogue;
    }
}