using UnityEngine;

// Ця структура дозволить нам налаштовувати рецепти прямо в редакторі
[System.Serializable]
public struct CraftRecipe
{
    [Tooltip("Ім'я предмета, на який треба кинути цей об'єкт")]
    public string targetItemName; 
    
    [Tooltip("Префаб нового об'єкта, який з'явиться")]
    public GameObject resultPrefab; 
}

public class CombinableItem : MonoBehaviour, IDraggable
{
    [Header("Налаштування предмета")]
    public string itemName; // Хто я такий (наприклад "Stick" або "Stone")
    public CraftRecipe[] recipes; // Список того, що я можу створити

    private Vector2 startPos;
    private Vector2 offset;
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    public void OnBeginDrag()
    {
        startPos = transform.position; // Запам'ятовуємо, звідки взяли

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)transform.position - mousePos;

        // РОБИМО ПРИВИДОМ: щоб не заблокувати собою предмет, на який падаємо
        myCollider.enabled = false; 
        transform.localScale *= 1.2f; // Візуальний фідбек
    }

    public void OnDrag(Vector2 worldPosition)
    {
        transform.position = worldPosition + offset; // Тягнемо
    }

    public void OnEndDrag()
    {
        transform.localScale /= 1.2f; // Повертаємо розмір

        bool craftSuccessful = false;

        // 1. Стріляємо лазером вниз: на що ми впали?
        Collider2D hit = Physics2D.OverlapPoint(transform.position);

        if (hit != null)
        {
            // Перевіряємо, чи впали ми на інший предмет для крафту
            CombinableItem otherItem = hit.GetComponent<CombinableItem>();
            
            if (otherItem != null)
            {
                // 2. Шукаємо в нашій "книзі" рецепт для цього предмета
                foreach (CraftRecipe recipe in recipes)
                {
                    if (recipe.targetItemName == otherItem.itemName)
                    {
                        // МАГІЯ КРАФТУ!
                        Debug.Log($"Схрестили {itemName} та {otherItem.itemName}!");
                        
                        // Створюємо новий предмет на місці того, на який ми впали
                        Instantiate(recipe.resultPrefab, otherItem.transform.position, Quaternion.identity);
                        
                        // Знищуємо обидва старі предмети
                        Destroy(otherItem.gameObject); // Знищуємо "Камінь"
                        Destroy(gameObject);           // Знищуємо "Палку"
                        
                        craftSuccessful = true;
                        break; // Зупиняємо пошук рецептів
                    }
                }
            }
        }

        // 3. Якщо крафт не вдався (впали не туди, або немає такого рецепта)
        if (!craftSuccessful)
        {
            transform.position = startPos; // Телепортуємо додому
            myCollider.enabled = true;     // Знову робимо фізичним
            Debug.Log("Не вийшло. Повертаємо на місце.");
        }
    }
}
