using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewCustomer", menuName = "WitchCafe/Customer")]
    public class CustomerData : ScriptableObject
    {
        [Header("Общие Данные")]
        [SerializeField] private string customerName;
        [SerializeField] private RecipeData desiredRecipe;
        [SerializeField] private bool isRecipeSecret;

        // --- НАШ ОБНОВЛЕННЫЙ БЛОК СПРАЙТОВ ---
        [Header("Визуальные Состояния")]
        [SerializeField] private Sprite neutralSprite;  // Когда зашел и делает заказ
        [SerializeField] private Sprite joyfulSprite;   // При успешной отдаче зелья
        [SerializeField] private Sprite sadSprite;      // Если игрок ошибся

        [Header("Диалоги")]
        [SerializeField] private DialogueData introDialogue;
        [SerializeField] private DialogueData successDialogue;
        [SerializeField] private DialogueData failureDialogue;

        // Публичные свойства для доступа из кода
        public string CustomerName => customerName;
        public RecipeData DesiredRecipe => desiredRecipe;
        public bool IsRecipeSecret => isRecipeSecret;

        public Sprite NeutralSprite => neutralSprite;
        public Sprite JoyfulSprite => joyfulSprite;
        public Sprite SadSprite => sadSprite;

        public DialogueData IntroDialogue => introDialogue;
        public DialogueData SuccessDialogue => successDialogue;
        public DialogueData FailureDialogue => failureDialogue;
    }
}