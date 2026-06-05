using UnityEngine;

namespace Core
{
    /// <summary>
    /// Базовый класс для реализации паттерна Singleton (Одиночка) в Unity.
    /// Зачем нужен: Он гарантирует, что в игре будет существовать только ОДИН экземпляр 
    /// конкретного менеджера (например, только один InventoryManager), и предоставляет 
    /// к нему глобальный доступ из любого другого скрипта через конструкцию "ИмяКласса.Instance".
    /// </summary>
    /// <typeparam name="T">Тип класса-менеджера, который становится синглтоном</typeparam>
    public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Закрытое статическое поле для хранения единственного экземпляра.
        // Переименовано в _instance в соответствии с правилами именования C#.
        private static T _instance;

        // Публичное свойство для доступа к менеджеру из других скриптов.
        public static T Instance
        {
            get
            {
                // Если экземпляра ещё нет в памяти...
                if (_instance == null)
                {
                    // ...пытаемся найти его на текущей сцене Unity 6
                    _instance = Object.FindFirstObjectByType<T>();
                    
                    // Если на сцене его тоже нет, создаем новый GameObject в runtime
                    if (_instance == null)
                    {
                        GameObject go = new GameObject($"[{typeof(T).Name}]");
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Виртуальный метод инициализации Unity. 
        /// Слово 'virtual' позволяет классам-наследникам переопределять этот метод (override),
        /// если им понадобится своя дополнительная логика при старте.
        /// </summary>
        protected virtual void Awake()
        {
            // Если экземпляр уже существует и это НЕ этот самый скрипт...
            if (_instance != null && _instance != this)
            {
                // ...значит, на сцене появилась копия менеджера. Уничтожаем её, чтобы избежать дублирования данных.
                Destroy(gameObject);
                return;
            }
            
            // Если всё хорошо, легитимизируем текущий экземпляр
            _instance = this as T;
        }
    }
}