using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void ClickBack()
    {
        // Знаходимо менеджера на сцені і кажемо йому працювати.
        // FindAnyObjectByType працює трохи повільно, але для кліку по UI — це абсолютно нормально.
        UIManager manager = FindAnyObjectByType<UIManager>();
        if (manager != null)
        {
            manager.GoBack();
        }
    }
}
