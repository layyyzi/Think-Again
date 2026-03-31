using UnityEngine;

public interface IDraggable
{
    void OnBeginDrag(); // Мишка натиснулась на об'єкт
    void OnDrag(Vector2 mousePosition); // Мишка рухається з натиснутою кнопкою
    void OnEndDrag(); // Відпустили мишку
}
