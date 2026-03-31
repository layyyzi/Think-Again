using UnityEngine;

public class WinObject : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        Debug.Log("Ціль знайдена! Викликаємо екран перемоги.");
        
        // Звертаємось до нашого менеджера, щоб він зупинив час і показав WinPanel
        LevelManager manager = FindAnyObjectByType<LevelManager>();
        
        if (manager is not null)
        {
            manager.ResultLevel();
        }
    }
}
