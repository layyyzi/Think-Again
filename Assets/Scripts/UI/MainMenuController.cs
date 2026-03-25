using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        gameObject.SetActive(false); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
