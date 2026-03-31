using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    // Робимо скрипт доступним звідусіль (Singleton)
    public static LevelManager Instance;

    [Header("Налаштування")]
    public GameObject[] levelPrefabs;
    public Transform levelHolder;
    public TextMeshProUGUI hudLevelText;
    [Header("UI Елементи")]
    public GameObject nextLevelButton;
    [Header("UI Тексти")]
    public TextMeshProUGUI objectiveText; // Сюди перетягнеш текст завдання з GameHUDPanel
    private string _currentHint;

    private GameObject _currentLevelInstance;
    private int _currentLevelIndex; // Тут ми будемо тримати номер поточного рівня

    private void Awake()
    {
        Instance = this;
    }

    public void LoadLevel(int levelIndex)
    {

        _currentLevelIndex = levelIndex;
        // 1. Очищуємо місце від старого рівня
        if (_currentLevelInstance != null)
        {
            Destroy(_currentLevelInstance);
        }

        // 2. Створюємо новий рівень (врахуй, що масиви починаються з 0, а рівні з 1)
        GameObject prefabToSpawn = levelPrefabs[levelIndex - 1];
        _currentLevelInstance = Instantiate(prefabToSpawn, levelHolder);
        
        LevelData data = _currentLevelInstance.GetComponent<LevelData>();
    
        if (data != null)
        {
            // 1. Одразу виводимо завдання на екран
            if (objectiveText != null) 
                objectiveText.text = data.objectiveDescription;
        
            // 2. Ховаємо підказку в кишеню Менеджера
            _currentHint = data.hintText;
        }
        else
        {
            Debug.LogWarning("На цьому рівні забули повісити скрипт LevelData!");
        }
        
        hudLevelText.text = "Level " + levelIndex;

        // 4. Перемикаємо UI з Вибору рівнів на Ігровий HUD
        FindAnyObjectByType<UIManager>().OpenScreen(FindAnyObjectByType<UIManager>().gameHudPanel);
    }
    
    // Додай це кудись вниз у LevelManager.cs

    public void PauseGame()
    {
        Time.timeScale = 0f; // Зупиняємо час у грі (всі рухи та анімації завмруть)
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Відновлюємо плин часу
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f; // ОБОВ'ЯЗКОВО повертаємо час, інакше меню теж зависне!
        
        // Знищуємо поточний рівень, щоб він не висів у пам'яті
        if (_currentLevelInstance != null)
        {
            Destroy(_currentLevelInstance);
        }
    }
    
    public void RestartLevel()
    {
        // 1. Обов'язково знімаємо гру з паузи, інакше новий рівень теж буде "замороженим"
        Time.timeScale = 1f; 

        // 2. Просто просимо менеджера завантажити той самий рівень ще раз!
        // (Метод LoadLevel сам знищить старі об'єкти і створить нові, ми це вже писали)
        LoadLevel(_currentLevelIndex); 
        
        // 3. Щоб гравець не міг випадково "повернутися" назад у меню паузи,
        // примусово скидаємо історію інтерфейсу і відкриваємо HUD
        FindAnyObjectByType<UIManager>().OpenRootScreen(FindAnyObjectByType<UIManager>().gameHudPanel);
    }
    
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Повертаємо час

        // Перевіряємо, чи є в нас наступний рівень у списку
        if (_currentLevelIndex < levelPrefabs.Length)
        {
            // currentLevelIndex у нас 1-базова, тому наступний буде currentLevelIndex + 1
            LoadLevel(_currentLevelIndex + 1);
        }
        else
        {
            // Якщо рівні закінчилися, повертаємо в меню вибору рівнів
            Debug.Log("Вітаємо! Ти пройшов усі рівні.");
            ExitToMenu();
            FindAnyObjectByType<UIManager>().OpenScreen(FindAnyObjectByType<UIManager>().levelSelectPanel);
        }
    }
    
    public void ResultLevel()
    {
        Time.timeScale = 0f; // Зупиняємо гру

        // МАГІЯ ТУТ: Якщо поточний номер рівня менший за загальну кількість префабів - кнопка увімкнена.
        // Якщо вони рівні (це останній рівень) - кнопка вимикається (зникає).
        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(_currentLevelIndex < levelPrefabs.Length);
        }

        // Відкриваємо екран перемоги як ПОПАП (поверх HUD)
        FindAnyObjectByType<UIManager>().ShowPopup(FindAnyObjectByType<UIManager>().resultPanel);
    }
    
    [Header("UI Панелі")]
    public TextMeshProUGUI popupHintText; // Текст всередині твоєї HintPanel

    // Цей метод викликатиме кнопка "?"
    public void ShowHint()
    {
        // Вставляємо текст із "кишені" у вікно підказки
        if (popupHintText != null)
        {
            popupHintText.text = _currentHint;
        }

        // Відкриваємо попап (ти вже писав цей метод для паузи)
        FindAnyObjectByType<UIManager>().ShowPopup(FindAnyObjectByType<UIManager>().hintPanel); // Припустимо, ти додав hintPanel в UIManager
    }
}
