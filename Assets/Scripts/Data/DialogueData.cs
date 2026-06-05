using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct DialogueLine
    {
        // Кто говорит (например, "Ведьма", "Бариста")
        [SerializeField] private string speakerName;
        
        // Что говорит
        [TextArea(3, 5)]
        [SerializeField] private string text;

        public string SpeakerName => speakerName;
        public string Text => text;
    }

    /// <summary>
    /// Данные отдельного диалога или сцены разговора.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "WitchCafe/Dialogue")]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private DialogueLine[] lines;

        public DialogueLine[] Lines => lines;
    }
}