using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Завдання для гравця")]
    // Атрибут TextArea робить поле в Інспекторі великим, щоб зручно було писати довгі речення
    [TextArea(2, 4)] 
    public string objectiveDescription = "Task";

    [Header("Підказка (для кнопки '?')")]
    [TextArea(3, 5)] 
    public string hintText = "Hint";
}
